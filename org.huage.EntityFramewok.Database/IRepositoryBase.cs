using System.Linq.Expressions;
namespace org.huage.EntityFramewok.Database;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    
    IQueryable<T> ExecuteSql(string sql);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}