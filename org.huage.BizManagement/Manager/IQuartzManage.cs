using org.huage.Entity.Request;
using Quartz;

namespace org.huage.BizManagement.Manager;

public interface IQuartzManage
{
    Task PauseJob(string jobName, string groupName);
    Task StartJob(string jobName, string groupName);
    Task AddJobForGet<T>(string jobName, string groupName, string remark, AddSchedulerRequest request) where T : IJob;

    Task AddJobForPost<T>(string jobName, string groupName, AddSchedulerRequest request)
        where T : IJob;

    Task UpdateJobRate(string jobName, string groupName, string remark, string cronExpression, long startTime);

    Task DeleteJob(string jobName, string groupName);
}