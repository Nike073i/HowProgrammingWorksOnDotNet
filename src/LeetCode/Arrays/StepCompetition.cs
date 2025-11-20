namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.StepCompetition;

/*
    task:
    - Соревнование по шагам. Соревнование идет несколько дней. Победителем считается тот, кто участвовал каждый день и набрал наибольшее кол-во шагов.
    - Информация записывается в виде [[[id, steps]]]
    time: O(k * n + nlogn), где k - кол-во дней, n - кол-во участников
    memory: O(n), где n - кол-во участников
*/
public class Solution
{
    public static List<int> GetWinners(int[][][] info)
    {
        int totalDays = info.Length;
        var scores = new Dictionary<int, int[]>();

        foreach (var day in info)
        {
            foreach (var participant in day)
            {
                int[] participantInfo = scores.GetValueOrDefault(participant[0], [0, 0]);
                participantInfo[0]++;
                participantInfo[1] += participant[1];
                scores[participant[0]] = participantInfo;
            }
        }

        int maxSteps = -1;
        foreach (var kvp in scores)
        {
            if (kvp.Value[0] == totalDays && kvp.Value[1] > maxSteps)
                maxSteps = kvp.Value[1];
        }

        var winners = new List<int>();
        foreach (var kvp in scores)
        {
            if (kvp.Value[0] == totalDays && kvp.Value[1] == maxSteps)
                winners.Add(kvp.Key);
        }
        return [.. winners.Order()];
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestGetWinners(int[][][] info, List<int> expected)
    {
        var actual = Solution.GetWinners(info);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[][][], List<int>>
{
    public SolutionTestData()
    {
        Add(
            [
                [
                    [1, 1000],
                    [2, 1500],
                ],
                [
                    [1, 1200],
                    [2, 1300],
                ],
                [
                    [1, 1100],
                    [2, 1400],
                ],
            ],
            [2]
        );

        Add(
            [
                [
                    [1, 1000],
                    [2, 1000],
                    [3, 800],
                ],
                [
                    [1, 1000],
                    [2, 1000],
                    [3, 900],
                ],
                [
                    [1, 1000],
                    [2, 1000],
                    [3, 700],
                ],
            ],
            [1, 2]
        );

        Add(
            [
                [
                    [1, 2000],
                    [2, 1500],
                ],
                [
                    [2, 1500],
                    [3, 5000],
                ],
                [
                    [1, 2000],
                    [2, 1500],
                ],
            ],
            [2]
        );

        Add(
            [
                [
                    [1, 500],
                    [2, 700],
                    [3, 700],
                ],
            ],
            [2, 3]
        );

        Add(
            [
                [
                    [1, 1000],
                    [2, 2000],
                ],
                [
                    [2, 2000],
                    [3, 3000],
                ],
                [
                    [1, 1000],
                    [3, 3000],
                ],
            ],
            []
        );

        Add(
            [
                [
                    [1, 5000],
                    [2, 1000],
                ],
                [
                    [1, 5000],
                    [2, 1000],
                ],
                [
                    [2, 1000],
                ],
            ],
            [2]
        );

        Add(
            [
                [
                    [5, 1000],
                    [3, 1000],
                    [1, 1000],
                ],
                [
                    [5, 1000],
                    [3, 1000],
                    [1, 1000],
                ],
                [
                    [5, 1000],
                    [3, 1000],
                    [1, 1000],
                ],
            ],
            [1, 3, 5]
        );
    }
}
