namespace HowProgrammingWorksOnDotNet.Exercises.Recursion;

public enum LabyrinthElement
{
    Free = 0,
    End = 1,
    Wall = 2,
}

public class LabyrinthTasks
{
    private readonly LabyrinthElement[][] _labyrinth;
    private readonly int _width;
    private readonly int _height;

    public LabyrinthTasks()
    {
        _labyrinth =
        [
            [
                LabyrinthElement.Free,
                LabyrinthElement.Free,
                LabyrinthElement.Wall,
                LabyrinthElement.Wall,
            ],
            [
                LabyrinthElement.Wall,
                LabyrinthElement.Free,
                LabyrinthElement.Free,
                LabyrinthElement.Free,
            ],
            [
                LabyrinthElement.Wall,
                LabyrinthElement.Wall,
                LabyrinthElement.Free,
                LabyrinthElement.End,
            ],
        ];
        _height = _labyrinth.Length;
        _width = _labyrinth[0].Length;
    }

    [Fact]
    public void FindWayOut()
    {
        var visited = new HashSet<(int x, int y)>(_width * _height);
        bool hasPath = HasPath(visited, 0, 0);
        Assert.True(hasPath);
    }

    private bool HasPath(HashSet<(int x, int y)> visited, int x, int y)
    {
        if (visited.Contains((x, y)))
            return false;

        if (
            x < 0
            || x >= _width
            || y < 0
            || y >= _height
            || _labyrinth[y][x] == LabyrinthElement.Wall
        )
            return false;

        if (_labyrinth[y][x] == LabyrinthElement.End)
            return true;

        visited.Add((x, y));
        return HasPath(visited, x, y + 1)
            || HasPath(visited, x, y - 1)
            || HasPath(visited, x + 1, y)
            || HasPath(visited, x - 1, y);
    }
}
