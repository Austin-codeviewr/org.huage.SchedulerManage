using Microsoft.EntityFrameworkCore;
using org.huage.EntityFramewok.Database.DBContext;
using org.huage.EntityFramewok.Database.Table;

namespace org.huage.EntityFramewok.Database.Repository.impl;

public class SchedulerRepository : RepositoryBase<Scheduler>,ISchedulerRepository
{
    public SchedulerRepository(SchedulerDbContext context) : base(context)
    {
    }
    
}