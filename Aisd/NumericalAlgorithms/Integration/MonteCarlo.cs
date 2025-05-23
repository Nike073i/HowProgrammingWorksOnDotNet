using System.Drawing;

namespace HowProgrammingWorksOnDotNet.Aisd.NumericalAlgorithms.Integration;

public class MonteCarloExample
{
    private record struct Point2D(double X, double Y);

    private class Circle(Point2D center, double r)
    {
        public bool IsPointBelongs(Point2D point)
        {
            double dx = center.X - point.X;
            double dy = center.Y - point.Y;
            return Math.Sqrt(dx * dx + dy * dy) < r;
        }
    }

    [Fact]
    public void GetCircleSquare()
    {
        double radius = 10;
        var circle = new Circle(new Point2D { X = -5, Y = -5 }, radius);

        var searchSpaceLeftUp = new Point2D { X = -15, Y = -15 };
        double searchSpaceWidth = 20;

        int searchAttempts = 10000;
        var random = new Random();
        Point2D GetSearchPoint()
        {
            double x = random.NextDouble() * searchSpaceWidth + searchSpaceLeftUp.X;
            double y = random.NextDouble() * searchSpaceWidth + searchSpaceLeftUp.Y;
            return new Point2D { X = x, Y = y };
        }
        var successTries = Enumerable
            .Range(0, searchAttempts)
            .Select(_ =>
            {
                var searchPoint = GetSearchPoint();
                return circle.IsPointBelongs(searchPoint);
            })
            .Count(x => x);
        var circleSquare = searchSpaceWidth * searchSpaceWidth * successTries / searchAttempts;
        Console.WriteLine($"Площадь по Монте-Карло: {circleSquare}");
        Console.WriteLine($"Реальная площадь: {radius * radius * Math.PI}");
    }
}
