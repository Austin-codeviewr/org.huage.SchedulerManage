using System.Text;
using Microsoft.Extensions.Logging;
using Quartz;

namespace org.huage.BizManagement.Job;

public class PostJob : IJob
{
    private readonly ILogger<PostJob> _logger;

    public PostJob(ILogger<PostJob> logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        //httpClient.DefaultRequestHeaders.Add("Portfolio", Portfolio);
        var url = context.MergedJobDataMap["RequestUrl"].ToString();
        var param = context.MergedJobDataMap["RequestParam"].ToString();
        
        var content = new StringContent(param ?? "{}", Encoding.UTF8, "application/json");

        try
        {
            var client = new HttpClient();
            using var httpResponse = await client.PostAsync(url, content);
            var result = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogInformation($"Executed the url: {url}, param is {param},Result is: "+ result);
        }
        catch (Exception e)
        {
            _logger.LogError($"执行请求地址：{url} 发生异常: {e.Message}");
        }
    }
}