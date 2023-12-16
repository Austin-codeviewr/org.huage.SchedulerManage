using AutoMapper;
using org.huage.BizManagement.Job;
using org.huage.BizManagement.Manager;
using org.huage.Entity.common;
using org.huage.Entity.Enum;
using org.huage.Entity.Model;
using org.huage.Entity.Request;

namespace org.huage.Api.Extension;

public class RestartRegisterScheduler : IHostedService
{
    private readonly ILogger<RestartRegisterScheduler> _logger;
    private readonly IServiceProvider _serviceProvider;

    public RestartRegisterScheduler(ILogger<RestartRegisterScheduler> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException("error");
    }

    /// <summary>
    /// 查詢所有的數據庫狀態為enable的，然後重新注冊。
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Register the Scheduler to task container.");

        using var scope = _serviceProvider.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<ISchedulerManager>();
        var quartzManage = scope.ServiceProvider.GetRequiredService<IQuartzManage>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        var request = new QuerySchedulerListByConditionsRequest()
        {
            Status = 0
        };
        var schedulerList = await manager.QuerySchedulerListByConditionsAsync(request);
        foreach (var scheduler in schedulerList.SchedulerModels)
        {
            if (request is null ||  (scheduler.RequestType != RequestType.REQUEST_TYPE_GET && scheduler.RequestType != RequestType.REQUEST_TYPE_POST))
                continue;
            var addSchedulerRequest = mapper.Map<SchedulerModel,AddSchedulerRequest>(scheduler);
            //注冊服務
            switch (scheduler.RequestType)
            { 
                case RequestType.REQUEST_TYPE_GET:
                    await quartzManage.AddJobForGet<GetJob>(SchedulerKeyGenerator.JobKey(scheduler.Id), SchedulerKeyGenerator.GroupKey(scheduler.Id)
                        , "",addSchedulerRequest);
                    break;
                case RequestType.REQUEST_TYPE_POST:
                    await quartzManage.AddJobForPost<PostJob>(SchedulerKeyGenerator.JobKey(scheduler.Id), SchedulerKeyGenerator.GroupKey(scheduler.Id), addSchedulerRequest);
                    break;
            }
        }
        _logger.LogInformation("BackGround Service has all register.");
        
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Schedulers Program is shut down,please check out.");
        return Task.CompletedTask;
    }
}