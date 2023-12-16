using org.huage.Entity.Enum;
using org.huage.Entity.Model;

namespace org.huage.Entity.Response;

public class QuerySchedulerListByConditionsResponse
{
    public List<SchedulerModel> SchedulerModels { get; set; } = new List<SchedulerModel>();
}