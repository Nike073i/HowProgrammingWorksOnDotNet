using System.Text;

namespace HowProgrammingWorksOnDotNet.Language.Strings;

public class Utf8Strings
{
    [Fact]
    public void Usage()
    {
        ReadOnlySpan<byte> utf8string = "this is text с русскими буквами"u8;
        Assert.Equal(47, utf8string.Length); // 15 латинских букв и пробелы, 16 русских букв, весом в 2 байта = 47 байт

        var unicodeString = Encoding.UTF8.GetString(utf8string);
        int unicodeByteLength = Encoding.Unicode.GetByteCount(unicodeString);
        Assert.Equal(31, unicodeString.Length); // 31 символ
        Assert.Equal(62, unicodeByteLength); // 31 символ * 2 = 62 байта
    }
}
