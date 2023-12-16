using org.huage.Entity.Model;

namespace org.huage.Entity.Response;

public class QuerySchedulerListResponse
{
    public List<SchedulerModel> Schedulers { get; set; } = new List<SchedulerModel>();
}