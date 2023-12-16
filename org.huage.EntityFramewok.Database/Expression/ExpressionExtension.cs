using System.Linq.Expressions;

namespace org.huage.EntityFramewok.Database;

public static class ExpressionExtension
{
    
    /// <summary>
    /// 初始化一个逻辑值为true的表达式
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <returns>新的表达式</returns>
    public static Expression<Func<TEntity, bool>> True<TEntity>()
    {
        return t => true;
    }

    /// <summary>
    /// 初始化一个逻辑值为false的表达式
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <returns>新的表达式</returns>
    public static Expression<Func<TEntity, bool>> False<TEntity>()
    {
        return t => false;
    }
    
    //生成逻辑与表达式
    public static Expression<Func<TEntity, bool>> And<TEntity>(this Expression<Func<TEntity, bool>> first, Expression<Func<TEntity, bool>> second)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "t");
        ParameterRebinder rebinder = new ParameterRebinder(parameter);
        Expression left = rebinder.RebindParameter(first.Body);
        Expression right = rebinder.RebindParameter(second.Body);
        Expression body = Expression.AndAlso(left, right);
        Expression<Func<TEntity, bool>> expression = Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        return expression;
    }
    
    /// <summary>
    /// 生成逻辑或表达式
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="first">第一个表达式</param>
    /// <param name="second">第二个表达式</param>
    /// <returns>新的表达式</returns>
    public static Expression<Func<TEntity, bool>> Or<TEntity>( Expression<Func<TEntity, bool>> first, Expression<Func<TEntity, bool>> second)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "t");
        ParameterRebinder rebinder = new ParameterRebinder(parameter);
        Expression left = rebinder.RebindParameter(first.Body);
        Expression right = rebinder.RebindParameter(second.Body);
        Expression body = Expression.OrElse(left, right);
        Expression<Func<TEntity, bool>> expression = Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        return expression;
    }
}