using Quartz;

namespace org.huage.BizManagement.Job;

public class HelloJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var url = context.MergedJobDataMap["RequestUrl"].ToString();
        var param = context.MergedJobDataMap["RequestParam"].ToString();
        Console.WriteLine("Hello，正在执行");
        Console.WriteLine($"Hello，{url}");
        Console.WriteLine($"Hello，{param}");
        var client = new HttpClient();
        using var httpResponse = await client.GetAsync(url);
        var result = await httpResponse.Content.ReadAsStringAsync();
        Console.WriteLine(result);
        
    }
}