using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using org.huage.BizManagement.Job;
using org.huage.BizManagement.Redis;
using org.huage.BizManagement.Wrapper;
using org.huage.Entity.common;
using org.huage.Entity.Enum;
using org.huage.Entity.Model;
using org.huage.Entity.Request;
using org.huage.Entity.Response;
using org.huage.EntityFramewok.Database;
using org.huage.EntityFramewok.Database.Table;
using SchedulerException = org.huage.Entity.common.SchedulerException;

namespace org.huage.BizManagement.Manager;

public class SchedulerManager : ISchedulerManager
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly ILogger<SchedulerManager> _logger;

    private readonly IRedisHelper _redisHelper;
    private IMapper _mapper;

    private readonly IQuartzManage _quartzManage;
    
    
    public SchedulerManager(IRepositoryWrapper wrapper, ILogger<SchedulerManager> logger, IMapper mapper,
        IRedisHelper redisHelper, IQuartzManage quartzManage)
    {
        _wrapper = wrapper;
        _logger = logger;
        _mapper = mapper;
        _redisHelper = redisHelper;
        _quartzManage = quartzManage;
    }
    
    public async Task<AddSchedulerResponse> AddSchedulerAsync(AddSchedulerRequest request)
    {
        //先保存到数据库，后续丢失可以重新跑
        AddSchedulerResponse response = new AddSchedulerResponse();
        try
        {
            if (request is null ||  (request.RequestType != RequestType.REQUEST_TYPE_GET && request.RequestType != RequestType.REQUEST_TYPE_POST))
                throw new SchedulerException("目前只支持Get,Post,请传入正确的RequestType.");
            //首先判断传进来的是get/post，然后在IJob定义向指定的接口发送请求。
            
            switch (request.RequestType)
            {
                case RequestType.REQUEST_TYPE_GET:
                    await _quartzManage.AddJobForGet<GetJob>(SchedulerKeyGenerator.JobKey(request.Id), SchedulerKeyGenerator.GroupKey(request.Id)
                        , "",request);
                    break;
                case RequestType.REQUEST_TYPE_POST:
                    await _quartzManage.AddJobForPost<PostJob>(SchedulerKeyGenerator.JobKey(request.Id), SchedulerKeyGenerator.GroupKey(request.Id), request);
                    break;
            }
            
            //删除缓存
            await _redisHelper.DelKey(RedisKeyGenerator.AllSchedulersRedisKey());
            
            //插入数据库
            var scheduler1 = _mapper.Map<Scheduler>(request);
            _wrapper.Scheduler.Create(scheduler1);
            await _wrapper.SaveChangeAsync();
            await _redisHelper.DelKey(RedisKeyGenerator.AllSchedulersRedisKey());
        }
        catch (Exception e)
        {
            _logger.LogError("AddSchedulerAsync occur error:{Message}", e.Message);
            throw;
        }

        return response;
    }

    /// <summary>
    /// 更新Scheduler Rate(调度速率)
    /// </summary>
    /// <param name="rateRequest"></param>
    /// <returns></returns>
    public async Task<UpdateSchedulerRateResponse> UpdateSchedulerRateAsync(UpdateSchedulerRateRequest rateRequest)
    {
        //先删除缓存
        await _redisHelper.DelKey(RedisKeyGenerator.AllSchedulersRedisKey());
        var schedulerUpdate = _mapper.Map<Scheduler>(rateRequest);

        schedulerUpdate.UpdateBy = "test";
        _wrapper.Scheduler.Update(schedulerUpdate);

        //1.移除原来的job; 2.修改trigger; 3.重新调度jobDetail

        await _quartzManage.UpdateJobRate(SchedulerKeyGenerator.JobKey(rateRequest.Id), SchedulerKeyGenerator.GroupKey(rateRequest.Id), rateRequest.Remark, rateRequest.CronExpression, rateRequest.StartTime);

        return new UpdateSchedulerRateResponse();
    }


    /// <summary>
    ///更新Scheduler状态，暂停，启用。
    /// </summary>
    /// <param name="statusRequest"></param>
    /// <returns></returns>
    /// <exception cref="SchedulerException"></exception>
    public async Task<bool> UpdateSchedulerStatusAsync(UpdateSchedulerStatusRequest statusRequest)
    {
        if (statusRequest is null || (statusRequest.Status != 0 && statusRequest.Status != 1))
            throw new SchedulerException("请传入正确的参数: Status.");

        switch (statusRequest.Status)
        {
            case 0:
                //执行关闭；
                await _quartzManage.PauseJob(SchedulerKeyGenerator.JobKey(statusRequest.Id), SchedulerKeyGenerator.GroupKey(statusRequest.Id));
                break;
            case 1:
                //执行启动
                await _quartzManage.StartJob(SchedulerKeyGenerator.JobKey(statusRequest.Id), SchedulerKeyGenerator.GroupKey(statusRequest.Id));
                break;
        }

        //删除缓存
        await _redisHelper.DelKey(RedisKeyGenerator.AllSchedulersRedisKey());

        //更新数据库
        var scheduler = _wrapper.Scheduler.FindByCondition(data => data.Id == statusRequest.Id && data.IsDeleted == false)
            .FirstOrDefault();
        if (scheduler != null)
        {
            scheduler.JobStatus = statusRequest.Status;
            _wrapper.Scheduler.Update(scheduler);
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// 更新整个JobDetail内容
    /// </summary>
    /// <param name="statusRequest"></param>
    /// <returns></returns>
    /// <exception cref="SchedulerException"></exception>
    public async Task<UpdateSchedulerResponse> UpdateSchedulerAsync(UpdateSchedulerRequest request)
    {
        
        //Delay double delete.
        await _redisHelper.DelKey(RedisKeyGenerator.AllSchedulersRedisKey());
        
        //更新数据库
        var updateScheduler = _mapper.Map<Scheduler>(request);
        _wrapper.Scheduler.Update(updateScheduler);
        
        await _redisHelper.DelKey(RedisKeyGenerator.AllSchedulersRedisKey());
        
        var response = new UpdateSchedulerResponse();
        //思路就是把trigger jobdetail全部替换成新的；1.先移除，2.新建（同时考虑会不会数据库重复，筛选条件加isDel）
        if (request is null ||  !"Get".Equals(request.RequestType,StringComparison.OrdinalIgnoreCase) || !"Post".Equals(request.RequestType,StringComparison.OrdinalIgnoreCase))
            throw new SchedulerException("目前只支持Get,Post,请传入正确的RequestType.");

        await _quartzManage.DeleteJob(SchedulerKeyGenerator.JobKey(request.Id),SchedulerKeyGenerator.GroupKey(request.Id));
        switch (request.RequestType)
        {
            case RequestType.REQUEST_TYPE_GET:
                await _quartzManage.AddJobForGet<GetJob>(SchedulerKeyGenerator.JobKey(request.Id), SchedulerKeyGenerator.GroupKey(request.Id)
                    , "", new AddSchedulerRequest());
                break;
            case RequestType.REQUEST_TYPE_POST:
                var map = _mapper.Map<AddSchedulerRequest>(request);
                await _quartzManage.AddJobForPost<PostJob>(SchedulerKeyGenerator.JobKey(request.Id), SchedulerKeyGenerator.GroupKey(request.Id),map);
                break;
        }
        
        
        return response;
    }

    /// <summary>
    /// 批量删除Schedulers
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    /// <exception cref="SchedulerException"></exception>
    public async Task<bool> BatchDelSchedulerAsync(List<Guid> ids)
    {
        if (!ids.Any())
            throw new SchedulerException("请传入正确的参数: ids.");
        //删除缓存
        await _redisHelper.DelKey(RedisKeyGenerator.AllSchedulersRedisKey());
        
        //更新数据库，逻辑删除
        var list = await _wrapper.Scheduler.FindByCondition(x => ids.Contains(x.Id)).ToListAsync();
        foreach (var scheduler in list)
        {
            scheduler.IsDeleted = true;
            scheduler.JobStatus = 0;
            _wrapper.Scheduler.Update(scheduler);
            //删除
            await _quartzManage.DeleteJob(SchedulerKeyGenerator.JobKey(scheduler.Id), SchedulerKeyGenerator.GroupKey(scheduler.Id));
        }
        await _wrapper.SaveChangeAsync();
        
        return true;
    }


    /// <summary>
    /// 查询所有的Scheduler列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<QuerySchedulerListResponse> QueryAllSchedulersAsync(QuerySchedulerListRequest request)
    {
        var response = new QuerySchedulerListResponse();
        try
        {
            //先去查redis
            var allSchedulers = await _redisHelper.HGetAllValue<Scheduler>(RedisKeyGenerator.AllSchedulersRedisKey());
            if (allSchedulers.Any())
            {
                var map = _mapper.Map<List<SchedulerModel>>(allSchedulers);
                response.Schedulers = map;
                return response;
            }

            //查数据库，并设置redis；
            var schedulers = _wrapper.Scheduler.FindAll().Where(_=>_.IsDeleted==false).ToList();
            var data = _mapper.Map<List<SchedulerModel>>(schedulers);
            response.Schedulers = data;
            var schedulersDic = schedulers.ToDictionary(_ => _.Id);
            
            var redisKey = RedisKeyGenerator.AllSchedulersRedisKey();
            await schedulersDic.ParallelForEachAsync(async scheduler =>
            {
                try
                {
                    foreach (var s in schedulers)
                    {
                        await _redisHelper.HashSet(redisKey, scheduler.Key.ToString(),
                            JsonConvert.SerializeObject(scheduler.Value));
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            });
        }
        catch (Exception e)
        {
            _logger.LogError($"QueryAllSchedulersAsync error: {e.Message}");
            throw;
        }

        return response;
    }


    /// <summary>
    /// 根据条件查找schedulers.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<QuerySchedulerListByConditionsResponse> QuerySchedulerListByConditionsAsync(QuerySchedulerListByConditionsRequest request)
    {
        var response = new QuerySchedulerListByConditionsResponse();
        try
        {
            //Param Check
            Expression<Func<Scheduler, bool>> expression = ExpressionExtension.True<Scheduler>();
            if (!string.IsNullOrEmpty(request.MethodName))
            {
                expression = expression.And(p => p.MethodName.Contains(request.MethodName));
            }
            if (!string.IsNullOrEmpty(request.Url))
            {
                expression = expression.And(p => p.Url.Contains(request.Url));
            }
            if (!string.IsNullOrEmpty(request.RequesType))
            {
                expression = expression.And(p => p.RequestType.Contains(request.RequesType));
            }

            expression=expression.And(p => p.IsDeleted == false);
            
            //查数据库
            var schedulers = await _wrapper.Scheduler.FindByCondition(expression)
                .OrderByDescending(_=>_.UpdateTime).ToListAsync();
            if (schedulers.Any())
            {
                var map = _mapper.Map<List<SchedulerModel>>(schedulers);
                response.SchedulerModels = map;
            }
            
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError($"QuerySchedulerListByConditions error:{e.Message}");
            throw;
        }
    }

    private void DelayedDoubleDeletion(string key)
    {
        _ = Task.Factory.StartNew(async () =>
        {
            await Task.Delay(3000);
            //删除key
            _redisHelper.Remove(key);
        });
    }
    
}