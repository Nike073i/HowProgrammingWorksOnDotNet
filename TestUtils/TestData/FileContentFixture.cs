namespace HowProgrammingWorksOnDotNet.TestUtils.TheoryData
{
    public abstract class FileContentFixture(string path)
    {
        public string Content { get; } = File.ReadAllText(path);
    }
}
