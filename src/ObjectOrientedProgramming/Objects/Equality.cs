using System.Diagnostics.CodeAnalysis;

namespace HowProgrammingWorksOnDotNet.ObjectOrientedProgramming.Objects;

public class EqualityTests
{
    private class A
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class AEqualityComparer : IEqualityComparer<A>
    {
        public bool Equals(A? x, A? y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] A obj) => obj.Id.GetHashCode();
    }

    [Fact]
    public void Example()
    {
        var a1 = new A { Id = 1, Name = "Value" };
        var a2 = new A { Id = 1, Name = "Value" };

        Assert.NotEqual(a1, a2);
        Assert.False(ReferenceEquals(a1, a2));

        Assert.Equal(a1, a2, new AEqualityComparer());
    }
}
