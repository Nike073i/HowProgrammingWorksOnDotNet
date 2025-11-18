namespace HowProgrammingWorksOnDotNet.TestUtils;

[CollectionDefinition(nameof(FileSystemCollection))]
public class FileSystemCollection : ICollectionFixture<FileSystemFixture>;

public class FileSystemFixture : IDisposable
{
    public FileSystemFixture() { }

    public void Dispose() { }
}

[Collection(nameof(FileSystemCollection))]
public class FileServiceTests(FileSystemFixture fixture)
{
    [Fact]
    public void CreateFile_ShouldCreateFileInTestDirectory()
    {
        // ...
    }
}
