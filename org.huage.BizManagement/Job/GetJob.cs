using Microsoft.Extensions.Logging;
using Quartz;

namespace org.huage.BizManagement.Job;

public class GetJob : IJob
{
    private readonly ILogger<GetJob> _logger;
    public GetJob(ILogger<GetJob> logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var url = context.MergedJobDataMap["RequestUrl"].ToString();
        var param = context.MergedJobDataMap["RequestParam"].ToString();
        try
        {
            var client = new HttpClient();
            using var httpResponse = await client.GetAsync(url);
            var result = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogInformation($"Executed the url: {url}, param is {param},Result is: "+ result);
        }
        catch (Exception e)
        {
            _logger.LogError($"执行请求地址：{url} 发生异常: {e.Message}");
        }
    }
}