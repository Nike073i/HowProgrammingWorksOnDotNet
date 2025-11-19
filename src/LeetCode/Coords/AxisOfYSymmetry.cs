namespace HowProgrammingWorksOnDotNet.LeetCode.Coords.AxisOfYSymmetry;

/*
    task: Проверить - можно ли провести линию OY, чтобы точки были симметричны относительно этой линии
    time: O(n)
    memory: O(n)
    notes:
    - Нужно не искать саму линию, а проверить, что у каждой точки есть симметричная пара
    - Особенность симметричных пар в том, что они равноудалены от краев диапазона. Например, в диапазоне [1, 4] точка 2 и 3 будут симметричны, поскольку каждая из удалена на 1 от границы
    - Диапазон определяется при помощи поиска минимальных и максимальных точек. При их нахождении, формула поиска пары приводится к `x_max - x_cur + x_min`
    - Все что нужно - проверить наличие пары
*/
public record Point2D(int X, int Y);

public class Solution
{
    public static bool IsSymmetric(Point2D[] points)
    {
        if (points.Length == 0)
            return false;
        if (points.Length == 1)
            return true;

        Point2D minPoint = points[0];
        Point2D maxPoint = points[0];
        for (int i = 1; i < points.Length; i++)
        {
            if (points[i].X < minPoint.X)
                minPoint = points[i];
            if (points[i].X > maxPoint.X)
                maxPoint = points[i];
        }

        if (minPoint.X == maxPoint.X)
            return true;
        if (minPoint.Y != maxPoint.Y)
            return false;

        var set = points.ToHashSet();
        foreach (var p in points)
        {
            var pair = new Point2D(maxPoint.X - p.X + minPoint.X, p.Y);
            if (!set.Contains(pair))
                return false;
        }
        return true;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestIsSymmetric(Point2D[] points, bool expected)
    {
        var actual = Solution.IsSymmetric(points);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<Point2D[], bool>
{
    public SolutionTestData()
    {
        Add([], false);
        Add([new Point2D(1, 2)], true);
        Add([new Point2D(1, 3), new Point2D(3, 3)], true);
        Add([new Point2D(1, 3), new Point2D(3, 4)], false);
        Add([new Point2D(1, 2), new Point2D(2, 5), new Point2D(3, 2)], true);
        Add([new Point2D(1, 2), new Point2D(2, 5), new Point2D(3, 3)], false);
        Add([new Point2D(2, 1), new Point2D(2, 3), new Point2D(2, 5)], true);
        Add([new Point2D(1, 1), new Point2D(1, 3), new Point2D(3, 1), new Point2D(3, 3)], true);
        Add([new Point2D(1, 1), new Point2D(1, 3), new Point2D(4, 1), new Point2D(4, 3)], true);
        Add([new Point2D(1, 1), new Point2D(2, 2), new Point2D(3, 3), new Point2D(4, 4)], false);
        Add(
            [
                new Point2D(1, 2),
                new Point2D(2, 4),
                new Point2D(3, 5),
                new Point2D(4, 4),
                new Point2D(5, 2),
            ],
            true
        );
        Add([new Point2D(0, 1), new Point2D(2, 2), new Point2D(4, 1)], true);
        Add([new Point2D(-2, 1), new Point2D(0, 3), new Point2D(2, 1)], true);
        Add([new Point2D(1, 2), new Point2D(1, 2), new Point2D(3, 2), new Point2D(3, 2)], true);
        Add([new Point2D(1, 1), new Point2D(1, 3), new Point2D(5, 1), new Point2D(5, 3)], true);
        Add([new Point2D(int.MinValue, 1), new Point2D(int.MaxValue, 1)], true);
        Add([new Point2D(2, 2), new Point2D(2, 2), new Point2D(2, 2)], true);
        Add([new Point2D(1, 1), new Point2D(2, 2), new Point2D(3, 1)], true);
        Add([new Point2D(2, 1), new Point2D(2, 3)], true);
        Add(
            [
                new Point2D(0, 0),
                new Point2D(1, 1),
                new Point2D(1, -1),
                new Point2D(2, 2),
                new Point2D(2, -2),
                new Point2D(3, 1),
                new Point2D(3, -1),
                new Point2D(4, 0),
            ],
            true
        );
    }
}
