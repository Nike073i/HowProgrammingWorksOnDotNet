using PointA = (int X, int Y);
using PointB = (int Z, int D);

// <DefineConstants>TYPE_ALIAS_FLAG</DefineConstants> в csproj
#if TYPE_ALIAS_FLAG
    using NumberList = System.Collections.Generic.List<int>;
#else
    using NumberList = int[];
#endif

// Псевдонимы работают только в рамках файлов. Если использовать CreatePoint в другом файле - вернется (int, int)
namespace HowProgrammingWorksOnDotNet.Language
{
    public class TypeAlias
    {
        public PointA CreatePoint(int x, int y) => new(x, y);

        [Fact]
        public void Usage()
        {
            NumberList coll = [1, 2, 3];
            Console.WriteLine(coll.GetType());

            var point1 = new PointB(10, 11);
            var point2 = CreatePoint(10, 11);
            point1.Z = 12;
            point2.X = 12;

            Assert.Equal(point1.GetType(), point2.GetType());
            Assert.False(ReferenceEquals(point1, point2));
            Assert.Equal(point1, point2);
        }
    }
}
