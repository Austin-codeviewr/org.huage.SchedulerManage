using org.huage.Entity.Request;
using org.huage.Entity.Response;

namespace org.huage.BizManagement.Manager;

public interface ISchedulerManager
{
    Task<QuerySchedulerListResponse> QueryAllSchedulersAsync(QuerySchedulerListRequest request);

    Task<AddSchedulerResponse> AddSchedulerAsync(AddSchedulerRequest request);

    Task<UpdateSchedulerRateResponse> UpdateSchedulerRateAsync(UpdateSchedulerRateRequest rateRequest);

    Task<bool> UpdateSchedulerStatusAsync(UpdateSchedulerStatusRequest statusRequest);

    Task<bool> BatchDelSchedulerAsync(List<Guid> ids);
    Task<QuerySchedulerListByConditionsResponse> QuerySchedulerListByConditionsAsync(QuerySchedulerListByConditionsRequest request);
    Task<UpdateSchedulerResponse> UpdateSchedulerAsync(UpdateSchedulerRequest request);
}