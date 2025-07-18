namespace HowProgrammingWorksOnDotNet.Aisd.Graph;

public record struct Point(double X, double Y)
{
    public readonly bool InRadius(Point another, double radius) =>
        Math.Pow(X - another.X, 2) + Math.Pow(Y - another.Y, 2) <= Math.Pow(radius, 2);

    public readonly Point GetCenter(Point second) => new((X + second.X) / 2, (Y + second.Y) / 2);
}

public enum Side
{
    None,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
}

public record struct Rectangle(Point TopLeft, Point BottomRight)
{
    private readonly Point Center => TopLeft.GetCenter(BottomRight);

    public readonly Rectangle FromSide(Side side) =>
        side switch
        {
            Side.TopLeft => new Rectangle(TopLeft, Center),
            Side.BottomRight => new Rectangle(Center, BottomRight),
            Side.TopRight => new Rectangle(
                new Point(Center.X, TopLeft.Y),
                new Point(BottomRight.X, Center.Y)
            ),
            Side.BottomLeft => new Rectangle(
                new Point(TopLeft.X, Center.Y),
                new Point(Center.X, BottomRight.Y)
            ),
            Side.None or _ => throw new InvalidOperationException(),
        };

    public readonly bool IsCircleIntersecting(Point center, double radius)
    {
        double closestX = Math.Clamp(center.X, TopLeft.X, BottomRight.X);
        double closestY = Math.Clamp(center.Y, TopLeft.Y, BottomRight.Y);

        return center.InRadius(new Point(closestX, closestY), radius);
    }

    public readonly Side SideOf(Point point)
    {
        if (point.X >= TopLeft.X && point.X < Center.X)
        {
            if (point.Y >= TopLeft.Y && point.Y < Center.Y)
                return Side.TopLeft;
            else
                return Side.BottomLeft;
        }
        else if (point.X >= Center.X && point.X < BottomRight.X)
        {
            if (point.Y >= TopLeft.Y && point.Y < Center.Y)
                return Side.TopRight;
            else
                return Side.BottomRight;
        }
        else
            return Side.None;
    }
}

public class QuadtreeTree<T>
{
    private record Element(T Value, Point Point);

    private interface IQuadtreeNode
    {
        void FindValues(Point target, double radius, List<Element> elements);
        void Add(Element element);
        Rectangle Rectangle { get; }
    }

    private class QuadtreeLeaf(int maxItem, Rectangle rectangle, Action<IQuadtreeNode> replaceMe)
        : IQuadtreeNode
    {
        public List<Element> Elements { get; } = [];

        public int MaxItems => maxItem;

        public Rectangle Rectangle => rectangle;

        public void Add(Element element)
        {
            Elements.Add(element);
            if (Elements.Count > maxItem)
                replaceMe(new QuadtreeComposite(maxItem, this));
        }

        public void FindValues(Point target, double radius, List<Element> elements) =>
            elements.AddRange(Elements.Where(e => e.Point.InRadius(target, radius)));
    }

    private class QuadtreeComposite : IQuadtreeNode
    {
        private IQuadtreeNode _nw;
        private IQuadtreeNode _sw;
        private IQuadtreeNode _ne;
        private IQuadtreeNode _se;
        public Rectangle Rectangle { get; }

        public QuadtreeComposite(int maxItem, QuadtreeLeaf quadtreeLeaf)
        {
            Rectangle = quadtreeLeaf.Rectangle;
            _nw = new QuadtreeLeaf(maxItem, Rectangle.FromSide(Side.TopLeft), (node) => _nw = node);
            _ne = new QuadtreeLeaf(
                maxItem,
                Rectangle.FromSide(Side.TopRight),
                (node) => _ne = node
            );
            _sw = new QuadtreeLeaf(
                maxItem,
                Rectangle.FromSide(Side.BottomLeft),
                (node) => _sw = node
            );
            _se = new QuadtreeLeaf(
                maxItem,
                Rectangle.FromSide(Side.BottomRight),
                (node) => _se = node
            );
            foreach (var el in quadtreeLeaf.Elements)
                Add(el);
            quadtreeLeaf.Elements.Clear();
        }

        public void Add(Element element)
        {
            IQuadtreeNode sideNode = Rectangle.SideOf(element.Point) switch
            {
                Side.TopLeft => _nw,
                Side.TopRight => _ne,
                Side.BottomLeft => _sw,
                Side.BottomRight => _se,
                _ => throw new InvalidOperationException(),
            };
            sideNode.Add(element);
        }

        public void FindValues(Point target, double radius, List<Element> elements)
        {
            var traverse = Traverse(target, radius, elements);
            traverse(_ne);
            traverse(_nw);
            traverse(_se);
            traverse(_sw);
        }

        private static Action<IQuadtreeNode> Traverse(
            Point target,
            double radius,
            List<Element> values
        ) =>
            node =>
            {
                if (node.Rectangle.IsCircleIntersecting(target, radius))
                    node.FindValues(target, radius, values);
            };
    }

    private IQuadtreeNode _root;

    public QuadtreeTree(int maxItem, Rectangle rectangle)
    {
        _root = new QuadtreeLeaf(maxItem, rectangle, node => _root = node);
    }

    public void Add(Point point, T value) => _root.Add(new Element(value, point));

    public IEnumerable<T> Find(Point target, double radius)
    {
        if (!_root.Rectangle.IsCircleIntersecting(target, radius))
            return [];
        var elements = new List<Element>();
        _root.FindValues(target, radius, elements);
        return elements.Select(e => e.Value);
    }
}

public class QuadtreeTreeTests
{
    [Fact]
    public void Usage()
    {
        var area = new Rectangle(new Point(0, 0), new Point(100, 100));
        var quadtree = new QuadtreeTree<string>(3, area);

        quadtree.Add(new Point(25, 25), "A");
        quadtree.Add(new Point(30, 30), "B");
        quadtree.Add(new Point(75, 25), "C");
        quadtree.Add(new Point(70, 30), "D");
        quadtree.Add(new Point(25, 75), "E");
        quadtree.Add(new Point(30, 70), "F");
        quadtree.Add(new Point(75, 75), "G");
        quadtree.Add(new Point(70, 70), "H");
        quadtree.Add(new Point(20, 40), "I");
        quadtree.Add(new Point(40, 55), "J");
        quadtree.Add(new Point(40, 90), "K");
        quadtree.Add(new Point(55, 80), "L");
        quadtree.Add(new Point(65, 45), "M");
        quadtree.Add(new Point(10, 60), "N");
        quadtree.Add(new Point(90, 35), "O");
        quadtree.Add(new Point(45, 70), "P");

        Assert.Empty(quadtree.Find(new Point(10, 30), 10));

        Assert.Single(quadtree.Find(new Point(10, 30), 15));

        Assert.Equal(3, quadtree.Find(new Point(10, 30), 20).Count());

        Assert.Single(quadtree.Find(new Point(75, 25), 1));

        Assert.Equal(6, quadtree.Find(new Point(40, 75), 20).Count());

        Assert.Equal(16, quadtree.Find(new Point(50, 50), 75).Count());
    }
}
