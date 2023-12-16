using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using org.huage.EntityFramewok.Database.DBContext;

namespace org.huage.EntityFramewok.Database;

public class RepositoryBase<T> : IRepositoryBase<T> where T: class
{
    private SchedulerDbContext _context;

    public RepositoryBase(SchedulerDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> FindAll()
    {
        return _context.Set<T>().AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression).AsNoTracking();
    }
    
    public IQueryable<T> ExecuteSql(string sql)
    {
        return _context.Set<T>().FromSqlRaw(sql);
    }

    public void Create(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}