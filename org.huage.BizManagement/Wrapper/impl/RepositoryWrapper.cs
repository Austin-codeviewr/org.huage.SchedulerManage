using org.huage.EntityFramewok.Database.DBContext;
using org.huage.EntityFramewok.Database.Repository;
using org.huage.EntityFramewok.Database.Repository.impl;

namespace org.huage.BizManagement.Wrapper.impl;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly SchedulerDbContext _context;

    private ISchedulerRepository _schedulerRepository;


    public ISchedulerRepository Scheduler
    {
        get { return _schedulerRepository ??= new SchedulerRepository(_context); }
    }


    public RepositoryWrapper(SchedulerDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangeAsync()
    {
        return _context.SaveChangesAsync();
    }
    
}