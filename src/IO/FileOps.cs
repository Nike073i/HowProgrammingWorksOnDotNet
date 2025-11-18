using System.Text;

namespace HowProgrammingWorksOnDotNet.IO;

public class FileOps
{
    private const string fileName = "output.txt";
    private const string content = "something text inside file";

    [Fact]
    public void UsageStatic()
    {
        try
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            using (var streamWriter = File.CreateText(fileName))
                streamWriter.WriteLine(content);

            var fileContent = File.ReadAllText(fileName, Encoding.UTF8);

            Assert.Equal(content + Environment.NewLine, fileContent);
        }
        finally
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }

    [Fact]
    public void UsageFileInfo()
    {
        var fileInfo = new FileInfo(fileName);

        try
        {
            if (fileInfo.Exists)
                fileInfo.Delete();

            using (var writer = fileInfo.CreateText())
                writer.WriteLine(content);

            /*
                Для не-текстовых данных используется Create
                using (var fileStream = fileInfo.Create()); 
                    ...

                Так можно создавать пустые файлы:
                fileInfo.Create().Close();
            */

            // Обновляем информацию о файле
            fileInfo.Refresh();

            // Проверяем свойства FileInfo
            Assert.True(fileInfo.Exists);
            Assert.True(fileInfo.Length > 0);
            Assert.Equal(fileName, fileInfo.Name);
            Assert.Equal(".txt", fileInfo.Extension);
            Assert.Equal(Path.GetFullPath(fileName), fileInfo.FullName);

            Console.WriteLine(fileInfo.Attributes);
            Console.WriteLine(fileInfo.CreationTimeUtc);
            Console.WriteLine(fileInfo.LastAccessTimeUtc);
            Console.WriteLine(fileInfo.LastWriteTimeUtc);

            using var reader = fileInfo.OpenText();

            /*
                Для не-текстовых данных используется Open
                using (var fileStream = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None));
                    ...
            */

            var fileContent = reader.ReadToEnd();
            Assert.Equal(content + Environment.NewLine, fileContent);
        }
        finally
        {
            if (fileInfo.Exists)
                fileInfo.Delete();
        }
    }
}
