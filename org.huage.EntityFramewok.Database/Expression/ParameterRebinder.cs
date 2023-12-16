using System.Linq.Expressions;

namespace org.huage.EntityFramewok.Database;

public class ParameterRebinder : ExpressionVisitor
{
    /// <summary>
    /// 类型参数
    /// </summary>
    private ParameterExpression parameter;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="parameter">类型参数</param>
    public ParameterRebinder(ParameterExpression parameter)
    {
        this.parameter = parameter;
    }

    /// <summary>
    /// 替换类型参数
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public Expression RebindParameter(Expression expression)
    {
        return Visit(expression);
    }

    /// <summary>
    /// 重写VisitParameter
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return this.parameter;
    }
}
