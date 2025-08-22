using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.SimplifyPath;

public class Solution
{
    public static string SimplifyPath(string path)
    {
        var stack = new Stack<string>();

        var segments = path[1..].Split("/");

        foreach (var segment in segments)
        {
            if (segment == string.Empty || segment == ".")
                continue;

            if (segment == "..")
            {
                if (stack.Count > 0)
                    stack.Pop();
            }
            else
                stack.Push(segment);
        }

        return CombinePath(stack);
    }

    private static string CombinePath(Stack<string> stack)
    {
        if (stack.Count == 0)
            return "/";

        string result = "";
        foreach (var dir in stack)
            result = $"/{dir}{result}";
        return result;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(string path, string expected)
    {
        string actual = Solution.SimplifyPath(path);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.TwoArg<string, string>
{
    public SolutionTestData()
    {
        Add("/", "/");
        Add("/home/", "/home");
        Add("/home", "/home");
        Add("//", "/");
        Add("/home//user/", "/home/user");
        Add("///", "/");
        Add("/home///docs", "/home/docs");
        Add("/./", "/");
        Add("/home/./user/", "/home/user");
        Add("/./././", "/");
        Add("/home/././docs", "/home/docs");
        Add("/../", "/");
        Add("/home/../", "/");
        Add("/home/user/../", "/home");
        Add("/home/../user", "/user");
        Add("/home/./../", "/");
        Add("/home/.././", "/");
        Add("/a/./b/../../c/", "/c");
        Add("/a/../../././", "/");
        Add("/home/user/documents/../pictures", "/home/user/pictures");
        Add("/a/b/c/../../../", "/");
        Add("/a/./b/./c/./d/", "/a/b/c/d");
        Add("/../../../../", "/");
        Add("/home/user/../../..", "/");
        Add("/a/b/../../c/d/../e", "/c/e");
        Add("/foo/bar", "/foo/bar");
        Add("/foo/bar/", "/foo/bar");
        Add("/foo/bar/..", "/foo");
        Add("/foo/bar/../baz", "/foo/baz");
        Add("/...", "/...");
        Add("/.../", "/...");
        Add("/....", "/....");
        Add("/..hidden", "/..hidden");
        Add("/.hidden", "/.hidden");
        Add("/foo-bar/baz_qux", "/foo-bar/baz_qux");
        Add("/123/456", "/123/456");
        Add("/with spaces/", "/with spaces");
        Add("/a/../../b/../c//.//", "/c");
        Add("/a//b////c/d//././/..", "/a/b/c");
        Add("/.././em/jl///../.././../E/", "/E");
        Add(
            "/very/long/path/with/many/directories/../and/./simplifications/../../needed",
            "/very/long/path/with/many/needed"
        );
        Add("/..", "/");
        Add("/./..", "/");
        Add("/../.", "/");
        Add("/././.././../", "/");
        Add("/.././.././", "/");
        Add("////////", "/");
        Add("/home////////user", "/home/user");
        Add("///tmp//////test///", "/tmp/test");
        Add("/file.txt", "/file.txt");
        Add("/folder/file.py", "/folder/file.py");
        Add("/config/.env", "/config/.env");
        Add("/.../.../...", "/.../.../...");
        Add("/..abc/def", "/..abc/def");
        Add("/a./b..", "/a./b..");
        Add("/.", "/");
        Add("/..", "/");
        Add("/...", "/...");
        Add("/home/", "/home");
        Add("/../", "/");
        Add("/home//foo/", "/home/foo");
        Add("/a/./b/../../c", "/c");
        Add("/a/b/c/d/e/f/../../../../g", "/a/b/g");
        Add("/1/2/3/4/5/6/7/8/9/../../../../../../../../", "/1");
        Add("/a/b/../c/../d/../e/../f", "/a/f");
        Add("/Home/User/Documents", "/Home/User/Documents");
        Add("/UPPER/lower/Mixed", "/UPPER/lower/Mixed");
    }
}
