using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public interface IProductOfOtherValuesSolution
{
    int[] Product(int[] input);
}

public abstract class ProductOfOtherValuesSolutionTests
{
    protected abstract IProductOfOtherValuesSolution CreateSolution();

    [Theory]
    [ClassData(typeof(ProductOfOtherValuesSolutionTestData))]
    public void Test(int[] input, int[] expected)
    {
        var solution = CreateSolution();
        var actual = solution.Product(input);
        Assert.Equal(actual, expected);
    }
}

public class ProductOfOtherValuesSolutionTestData : TheoryDataContainer.TwoArg<int[], int[]>
{
    public ProductOfOtherValuesSolutionTestData()
    {
        Add([1, 2, 3, 4], [24, 12, 8, 6]);
        Add([1, 0, 3, 4], [0, 12, 0, 0]);
        Add([0, 2, 3, 4], [24, 0, 0, 0]);
        Add([1, 2, 0, 4], [0, 0, 8, 0]);
        Add([1, 0, 3, 0], [0, 0, 0, 0]);
        Add([-1, 2, -3, 4], [-24, 12, -8, 6]);
        Add([0, 1, 2, 3], [6, 0, 0, 0]);
        Add([1, 2, 0, 4, 5], [0, 0, 40, 0, 0]);
        Add([1, 2, 3, 0], [0, 0, 0, 6]);
    }
}

public class PrefixSuffixProductOfOtherValuesTests : ProductOfOtherValuesSolutionTests
{
    protected override IProductOfOtherValuesSolution CreateSolution() =>
        new PrefixSuffixProductOfOtherValues();
}

public class PrefixSuffixProductOfOtherValues : IProductOfOtherValuesSolution
{
    public int[] Product(int[] input)
    {
        int[] res = new int[input.Length];
        res[0] = 1;
        for (int i = 1; i < res.Length; i++)
            res[i] = res[i - 1] * input[i - 1];

        int suf = 1;
        for (int i = input.Length - 1; i >= 0; i--)
        {
            res[i] *= suf;
            suf *= input[i];
        }
        return res;
    }
}

public class MutationProductOfOtherValuesTests : ProductOfOtherValuesSolutionTests
{
    protected override IProductOfOtherValuesSolution CreateSolution() =>
        new MutationProductOfOtherValues();
}

public class MutationProductOfOtherValues : IProductOfOtherValuesSolution
{
    public int[] Product(int[] input)
    {
        int zeroIndex = -1;
        int product = 1;

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == 0)
            {
                if (zeroIndex != -1)
                {
                    Array.Fill(input, 0);
                    return input;
                }
                else
                    zeroIndex = i;
            }
            else
                product *= input[i];
        }

        if (zeroIndex != -1)
        {
            Array.Fill(input, 0);
            input[zeroIndex] = product;
            return input;
        }

        for (int i = 0; i < input.Length; i++)
            input[i] = product / input[i];

        return input;
    }
}
