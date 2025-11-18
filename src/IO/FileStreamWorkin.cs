using System.Text;
using Microsoft.CodeAnalysis;

namespace HowProgrammingWorksOnDotNet.IO;

public class FileStreamWorking
{
    private string fileName = "tmp.txt";
    private string text = "HelloWorld фром Руссиа";

    [Fact]
    public void WriteTextByByte()
    {
        using (Stream fstream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
        {
            var content = Encoding.UTF8.GetBytes(text);
            fstream.Write(content);

            // Чтение из того-же потока. Просто пример. Но требуется, чтобы FileAccess был ReadWrite
            byte[] repeatedContent = new byte[content.Length];
            fstream.Position = 0;
            fstream.ReadExactly(repeatedContent);
            var repeatedText = Encoding.UTF8.GetString(repeatedContent);
            Assert.Equal(text, repeatedText);
        }
        Assert.True(File.Exists(fileName));
        var fileContent = File.ReadAllText(fileName);
        Assert.Equal(fileContent, text);
        File.Delete(fileName);
    }

    [Fact]
    public void WriteTextByWriter()
    {
        using (var writer = new StreamWriter(fileName))
            writer.Write(text);

        Assert.True(File.Exists(fileName));

        // var fileContent = File.ReadAllText(fileName);

        using (var reader = new StreamReader(fileName))
        {
            var fileContent = reader.ReadToEnd();
            Assert.Equal(fileContent, text);
        }
        File.Delete(fileName);
    }
}
