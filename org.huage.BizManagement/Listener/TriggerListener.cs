using Quartz;

namespace org.huage.BizManagement.Listener;

public class TriggerListener : ITriggerListener
{
    public string Name { get; } = nameof(TriggerListener);
    public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
    public Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
    public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
    public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);   //返回true表示否决Job继续执行
    }
}