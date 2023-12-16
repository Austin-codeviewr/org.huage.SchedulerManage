using org.huage.EntityFramewok.Database.Repository;

namespace org.huage.BizManagement.Wrapper;

public interface IRepositoryWrapper
{
    ISchedulerRepository Scheduler { get; }
    
    Task<int> SaveChangeAsync();
    
}