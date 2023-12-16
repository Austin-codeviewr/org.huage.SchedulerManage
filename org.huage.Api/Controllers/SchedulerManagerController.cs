using Microsoft.AspNetCore.Mvc;
using org.huage.BizManagement.Manager;
using org.huage.Entity.common;
using org.huage.Entity.Request;
using org.huage.Entity.Response;

namespace org.huage.Api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class SchedulerManagerController
{
    private readonly ISchedulerManager _manager;

    private readonly ILogger<SchedulerManagerController> _logger;

    public SchedulerManagerController(ISchedulerManager manager, ILogger<SchedulerManagerController> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    [HttpPost]
    public async Task<AddSchedulerResponse> AddScheduler(AddSchedulerRequest request)
    {
        try
        {
            return await _manager.AddSchedulerAsync(request);
        }
        catch (SchedulerException exception)
        {
            _logger.LogError(
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.AddScheduler.");
            throw;
        }
        catch (Exception e)
        {
            var schedulerException = new SchedulerException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.AddScheduler.");
            throw schedulerException;
        }
    }

    [HttpPost]
    public async Task<QuerySchedulerListResponse> QueryAllSchedulers(QuerySchedulerListRequest request)
    {
        try
        {
            return await _manager.QueryAllSchedulersAsync(request);
        }
        catch (SchedulerException exception)
        {
            _logger.LogError($"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.AddScheduler.");
            throw;
        }
        catch (Exception e)
        {
            var schedulerException = new SchedulerException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.AddScheduler.");
            throw schedulerException;
        }
    }


    [HttpPost]
    public async Task<UpdateSchedulerRateResponse> UpdateSchedulerRate(UpdateSchedulerRateRequest rateRequest)
    {
        try
        {
            return await _manager.UpdateSchedulerRateAsync(rateRequest);
        }
        catch (SchedulerException exception)
        {
            _logger.LogError(
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.UpdateScheduler.");
            throw;
        }
        catch (Exception e)
        {
            var schedulerException = new SchedulerException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.UpdateScheduler.");
            throw schedulerException;
        }
    }

    [HttpPost]
    public async Task<bool> UpdateSchedulerStatus(UpdateSchedulerStatusRequest statusRequest)
    {
        try
        {
            return await _manager.UpdateSchedulerStatusAsync(statusRequest);
        }
        catch (SchedulerException exception)
        {
            _logger.LogError(
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.UpdateSchedulerStatus.");
            throw;
        }
        catch (Exception e)
        {
            var schedulerException = new SchedulerException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.UpdateSchedulerStatus.");
            throw schedulerException;
        }
    }

    [HttpPost]
    public async Task<bool> BatchDelScheduler(List<Guid> ids)
    {
        try
        {
            return await _manager.BatchDelSchedulerAsync(ids);
        }
        catch (SchedulerException exception)
        {
            _logger.LogError(
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.BatchDelScheduler.");
            throw;
        }
        catch (Exception e)
        {
            var schedulerException = new SchedulerException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.BatchDelScheduler.");
            throw schedulerException;
        }
    }

    [HttpPost]
    public async Task<QuerySchedulerListByConditionsResponse> QuerySchedulerListByConditions(
        QuerySchedulerListByConditionsRequest request)
    {
        try
        {
            return await _manager.QuerySchedulerListByConditionsAsync(request);
        }
        catch (SchedulerException exception)
        {
            _logger.LogError(
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.QuerySchedulerListByConditions.");
            throw;
        }
        catch (Exception e)
        {
            var schedulerException = new SchedulerException(e.Message);
            _logger.LogError(e,
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.QuerySchedulerListByConditions.");
            throw schedulerException;
        }
    }

    [HttpPost]
    public async Task<UpdateSchedulerResponse> UpdateScheduler(UpdateSchedulerRequest request)
    {
        try
        {
            return await _manager.UpdateSchedulerAsync(request);
        }
        catch (SchedulerException exception)
        {
            _logger.LogError(
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.UpdateScheduler.");
            throw;
        }
        catch (Exception e)
        {
            var schedulerException = new SchedulerException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.SchedulerManager.SchedulerManagerController.UpdateScheduler.");
            throw schedulerException;
        }
    }
    
}