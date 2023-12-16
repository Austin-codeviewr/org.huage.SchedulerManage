namespace org.huage.BizManagement.Redis;

public static class RedisServiceExtensions
{
    public static async Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> asyncAction, int maxDegreeOfParallelism = 10)
    {
        var throttler = new SemaphoreSlim(initialCount: maxDegreeOfParallelism);
        var tasks = source.Select(async item =>
        {
            await throttler.WaitAsync();
            try
            {
                await asyncAction(item).ConfigureAwait(false);
            }
            finally
            {
                throttler.Release();
            }
        });
        await Task.WhenAll(tasks);
    }
}