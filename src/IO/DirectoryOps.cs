namespace HowProgrammingWorksOnDotNet.IO;

public class DirectoryOps
{
    private const string testDir = "TestDirectory";
    private const string subDir = "SubDirectory";
    private const string fileName = "test.txt";

    [Fact]
    public void UsageDirectoryStatic()
    {
        try
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, recursive: true);

            Directory.CreateDirectory(testDir);
            Assert.True(Directory.Exists(testDir));

            string subDirPath = Path.Combine(testDir, subDir);
            Directory.CreateDirectory(subDirPath);
            Assert.True(Directory.Exists(subDirPath));

            string filePath = Path.Combine(testDir, fileName);
            File.WriteAllText(filePath, "test content");

            var files = Directory.GetFiles(testDir);
            Assert.Single(files);
            Assert.Equal(filePath, files[0]);

            var directories = Directory.GetDirectories(testDir);
            Assert.Single(directories);
            Assert.Equal(subDirPath, directories[0]);

            var parentDir = Directory.GetParent(testDir);
            Assert.NotNull(parentDir);

            var currentDir = Directory.GetCurrentDirectory(); // proccess current directory
            Assert.NotNull(currentDir);
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, recursive: true);
        }
    }

    [Fact]
    public void UsageDirectoryInfo()
    {
        var dirInfo = new DirectoryInfo(testDir);

        try
        {
            if (dirInfo.Exists)
                dirInfo.Delete(true);

            dirInfo.Create();
            dirInfo.Refresh();
            Assert.True(dirInfo.Exists);

            var subDirInfo = dirInfo.CreateSubdirectory(subDir);
            Assert.True(subDirInfo.Exists);
            Assert.Equal(subDir, subDirInfo.Name);

            string filePath = Path.Combine(dirInfo.FullName, fileName);
            File.WriteAllText(filePath, "test content");

            dirInfo.Refresh();

            var files = dirInfo.GetFiles("*.txt", SearchOption.AllDirectories); // recursive
            Assert.Single(files);
            Assert.Equal(fileName, files[0].Name);

            var directories = dirInfo.GetDirectories();
            Assert.Single(directories);
            Assert.Equal(subDir, directories[0].Name);

            Assert.Equal(testDir, dirInfo.Name);
            Assert.True(dirInfo.CreationTime > DateTime.Now.AddMinutes(-1));

            var parentDir = dirInfo.Parent;
            Assert.NotNull(parentDir);

            var rootDir = dirInfo.Root;
            Assert.NotNull(rootDir);
        }
        finally
        {
            if (dirInfo.Exists)
                dirInfo.Delete(true);
        }
    }

    [Fact]
    public void FindFilesByPattern()
    {
        var dir = new DirectoryInfo("/storage");
        var files = dir.GetFiles("*.md", SearchOption.AllDirectories); // count of markdown docs
        Console.WriteLine(files.Length);
    }
}
