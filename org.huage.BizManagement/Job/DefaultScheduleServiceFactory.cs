using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace org.huage.BizManagement.Job;

public class DefaultScheduleServiceFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DefaultScheduleServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        //返回jobType对应类型的实例
        return  _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
    }

    public void ReturnJob(IJob job)
    {
        var disposable = job as IDisposable;

        disposable?.Dispose();
    }
}