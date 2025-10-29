namespace HowProgrammingWorksOnDotNet.Async;

public class Cancelling
{
    [Fact]
    public async Task UsageTimeout()
    {
        async Task<string> GetContent(double delaySeconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            return "content";
        }

        Func<Task> slowApi = async () => await GetContent(2).WaitAsync(TimeSpan.FromSeconds(1));
        Func<Task> fastApi = async () => await GetContent(0.5).WaitAsync(TimeSpan.FromSeconds(1));

        await Assert.ThrowsAsync<TimeoutException>(slowApi);

        await fastApi();
    }
}
