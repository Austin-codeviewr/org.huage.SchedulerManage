using Microsoft.Extensions.Logging;
using Quartz;

namespace org.huage.BizManagement.Job;

public class JobListener : IJobListener
{
    private ILogger<JobListener> _logger;

    public JobListener(ILogger<JobListener> logger)
    {
        _logger = logger;
    }

    public JobListener()
    {
    }

    public Task JobToBeExecuted(IJobExecutionContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.Factory.StartNew(() =>
        {
            _logger.LogInformation("Job: {JobDetailKey} 即将执行", context.JobDetail.Key);
        });
    }

    public Task JobExecutionVetoed(IJobExecutionContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.Factory.StartNew(() =>
        {
            _logger.LogInformation("Job: {JobDetailKey} 被否决执行", context.JobDetail.Key);
        });
    }

    public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.Factory.StartNew(() =>
        {
            _logger.LogInformation("Job: {JobDetailKey} 执行完成", context.JobDetail.Key);
        });
    }

    public string Name { get; } = nameof(JobListener);
}