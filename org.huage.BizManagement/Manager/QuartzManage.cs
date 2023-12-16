using System.Collections.Specialized;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using org.huage.BizManagement.Job;
using org.huage.BizManagement.Listener;
using org.huage.BizManagement.Proxy;
using org.huage.Entity.common;
using org.huage.Entity.Request;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using SchedulerException = Quartz.SchedulerException;

namespace org.huage.BizManagement.Manager;

public class QuartzManage : IQuartzManage
{
    private static ISchedulerFactory _schedulerFactory;
    private static IScheduler _scheduler;
    private readonly IJobFactory _jobFactory;
    
    public QuartzManage(IJobFactory jobFactory)
    {
        _jobFactory = jobFactory;
        
        var config = new NameValueCollection();
        config.Add("quartz.jobStore.misfireThreshold", "1000");
        
        _schedulerFactory = new StdSchedulerFactory(config);
        //获得调度器
        _scheduler = _schedulerFactory.GetScheduler().Result;
        
        _scheduler.JobFactory = jobFactory;
        //调度开始
        _scheduler.Start();
    }
    
    
    /// <summary>
    /// 判断corn表达式是否有效
    /// </summary>
    /// <param name="cronExpression"></param>
    /// <returns></returns>
    private bool IsValidExpression(string cronExpression)
    {
        CronTriggerImpl trigger = new CronTriggerImpl();
        try
        {
            trigger.CronExpressionString = cronExpression;
            DateTimeOffset? date = trigger.ComputeFirstFireTimeUtc(null);
            return date != null;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    /// <summary>
    /// 暂停Job
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jobName"></param>
    /// <param name="groupName"></param>
    public async Task PauseJob(string jobName, string groupName)
    {
        //查询同一个分组的JobKey列表
        var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName));
        if (jobKeys != null && jobKeys.Count > 0)
        {
            //获得指定名称jobName的的jobKey
            var jobKey = jobKeys.FirstOrDefault(p => p.Name.Equals(jobName));
            if (jobKey != null)
            {
                //暂停job的调度
                await _scheduler.PauseJob(jobKey);
            }
            else
            {
                throw new SchedulerException("Can' find this Job.");
            }
        }
        else
        {
            throw new SchedulerException("Can't fin this Job.");
        }
    }
    
    /// <summary>
    /// 重启job
    /// </summary>
    /// <param name="jobName"></param>
    /// <param name="groupName"></param>
    /// <exception cref="Exception"></exception>
    public async Task StartJob(string jobName, string groupName)
    {
        //查询同一个分组的JobKey列表
        var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName));
        if (jobKeys != null && jobKeys.Any())
        {
            //获得指定名称jobName的的jobKey
            var jobKey = jobKeys.FirstOrDefault(p => p.Name.Equals(jobName));
            if (jobKey != null)
            {
                //暂停job的调度
                await _scheduler.ResumeJob(jobKey);
            }
            else
            {
                throw new SchedulerException("没有此Job");
            }
        }
        else
        {
            throw new SchedulerException("没有此Job");
        }
    }
    
    /// <summary>
    /// 创建Get请求方式Job
    /// </summary>
    /// <param name="jobName"></param>
    /// <param name="groupName"></param>
    /// <param name="desc"></param>
    /// <param name="cronExpression"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="Exception"></exception>
    public async Task AddJobForGet<T>(string jobName, string groupName, string remark,AddSchedulerRequest request)
        where T : IJob
    {
        //判断 cron 表达式是否有效
        if (!IsValidExpression(request.CronExpression))
        {
            throw new Exception("请输入cron表达式");
        }
        //判断url
        if (string.IsNullOrEmpty(request.Url) || ! Uri.TryCreate(request.Url,UriKind.Absolute,out var success))
        {
            throw new Exception($"当前Url 格式不正确：{request.Url}");
        }
        //创建Job
        var job = JobBuilder.Create<T>().UsingJobData(new JobDataMap()
            {
                new KeyValuePair<string, object>("RequestUrl", request.Url),
                new KeyValuePair<string, object>("RequestParam", request.MethodParams)
            })
            .WithIdentity(jobName, groupName)//键值
            .WithDescription(remark)//描述
            .Build();

        //创建触发器
        var timeUnit = SchedulerUtils.GetTimeUnit(request.StartTime);
        
        ITrigger trigger = TriggerBuilder.Create()
            .WithCronSchedule(request.CronExpression,x=>x.WithMisfireHandlingInstructionFireAndProceed())
            .WithIdentity(jobName, groupName)
            .StartAt(DateBuilder.DateOf(timeUnit[3], timeUnit[4], 
                timeUnit[5],timeUnit[2],timeUnit[1],timeUnit[0]))
            .Build();

        //添加监听器
        //_scheduler.ListenerManager.AddJobListener(new JobListener(),GroupMatcher<JobKey>.AnyGroup());
        //_scheduler.ListenerManager.AddTriggerListener(new TriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());
        //开始以创建的触发器调度创建的Job
        
        await _scheduler.ScheduleJob(job, trigger);
    }
    
    /// <summary>
    /// 创建post请求的参数
    /// </summary>
    /// <param name="jobName"></param>
    /// <param name="groupName"></param>
    /// <param name="request"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="Exception"></exception>
    public async Task AddJobForPost<T>(string jobName, string groupName,AddSchedulerRequest request)
        where T : IJob
    {
        //判断 cron 表达式是否有效
        if (!IsValidExpression(request.CronExpression))
        {
            throw new Exception("请输入cron表达式");
        }
        //创建Job
        var job = JobBuilder.Create<T>()
            .WithIdentity(jobName, groupName).UsingJobData(new JobDataMap()
            {
                new KeyValuePair<string, object>("RequestUrl", request.Url),
                new KeyValuePair<string, object>("RequestParam", request.MethodParams)
            }).RequestRecovery(true).WithDescription(request.Remark ?? "")
            .Build();

        //创建触发器
        var timeUnit = SchedulerUtils.GetTimeUnit(request.StartTime);
        ITrigger trigger = TriggerBuilder.Create()
            .WithCronSchedule(request.CronExpression,x=>x.WithMisfireHandlingInstructionFireAndProceed())
            .WithIdentity(SchedulerKeyGenerator.TriggerKey(request.Id), groupName)
            .StartAt(DateBuilder.DateOf(timeUnit[3], timeUnit[4], 
                timeUnit[5],timeUnit[2],timeUnit[1],timeUnit[0]))
            .Build();

        //添加监听器
        //_scheduler.ListenerManager.AddJobListener(new JobListener(),GroupMatcher<JobKey>.AnyGroup());
        //_scheduler.ListenerManager.AddTriggerListener(new TriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());
        
        await _scheduler.ScheduleJob(job, trigger);
    }
    
    
    /// <summary>
    /// 更新job调度频率
    /// </summary>
    /// <param name="jobName"></param>
    /// <param name="groupName"></param>
    /// <param name="remark"></param>
    /// <param name="cronExpression"></param>
    /// <exception cref="Exception"></exception>
    /// <exception cref="SchedulerException"></exception>
    public async Task UpdateJobRate(string jobName, string groupName, string remark, string cronExpression,long startTime)
    {
        if (!IsValidExpression(cronExpression))
        {
            throw new Exception("cron表达式无效");
        }

        var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName));
        if (jobKeys != null && jobKeys.Any())
        {
            var jobKey = jobKeys.FirstOrDefault(p => p.Name.Equals(jobName));
            if (jobKey != null)
            {
                var job = await _scheduler.GetJobDetail(jobKey);
                if (job == null) throw new Exception("job不存在");
                //先删除原有job
                await _scheduler.DeleteJob(jobKey);
                
                var timeUnit = SchedulerUtils.GetTimeUnit(startTime);
        
                var trigger = TriggerBuilder.Create()
                    .WithCronSchedule(cronExpression)
                    .WithIdentity(jobName, groupName)
                    .StartAt(DateBuilder.DateOf(timeUnit[3], timeUnit[4], 
                        timeUnit[5],timeUnit[2],timeUnit[1],timeUnit[0]))
                    .Build();
                await _scheduler.ScheduleJob(job, trigger);
            }
            else
            {
                throw new SchedulerException("job不存在");
            }
        }
        else
        {
            throw new SchedulerException("job不存在");
        }
    }
    
    
    /// <summary>
    /// 删除job
    /// </summary>
    /// <param name="jobName"></param>
    /// <param name="groupName"></param>
    /// <exception cref="Exception"></exception>
    public async Task DeleteJob(string jobName, string groupName)
    {
        var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName));
        if (jobKeys != null && jobKeys.Any())
        {
            var jobKey = jobKeys.FirstOrDefault(p => p.Name.Equals(jobName));
            if (jobKey != null)
            {
                await _scheduler.DeleteJob(jobKey);
            }
            else
            {
                throw new SchedulerException("要删除的job不存在");
            }
        }
        else
        {
            throw new SchedulerException("要删除的job不存在");
        }
    }
}
     
