namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.TouristRoute;

/*
    task: Есть билеты туриста вида (Откуда -> Куда). Нужно составить полный маршрут. Важно, что каждый город был посещен лишь один раз
    time: O(n)
    memory: O(n)
*/
public class Solution
{
    public static List<char> GetPath(char[][] tickets)
    {
        var routes = new Dictionary<char, char>();
        var endpoints = new HashSet<char>();

        foreach (var ticket in tickets)
        {
            routes[ticket[0]] = ticket[1];
            endpoints.Add(ticket[1]);
        }
        char startPoint = '0';
        foreach (var ticket in tickets)
        {
            if (!endpoints.Contains(ticket[0]))
            {
                startPoint = ticket[0];
                break;
            }
        }
        var output = new List<char>() { startPoint };

        while (routes.ContainsKey(output[^1]))
            output.Add(routes[output[^1]]);

        return output;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestGetPath(char[][] tickets, List<char> expected)
    {
        var actual = Solution.GetPath(tickets);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<char[][], List<char>>
{
    public SolutionTestData()
    {
        Add(
            [
                ['A', 'B'],
                ['B', 'C'],
                ['C', 'D'],
            ],
            ['A', 'B', 'C', 'D']
        );

        Add(
            [
                ['A', 'B'],
            ],
            ['A', 'B']
        );

        Add(
            [
                ['A', 'B'],
                ['B', 'C'],
            ],
            ['A', 'B', 'C']
        );
    }
}
