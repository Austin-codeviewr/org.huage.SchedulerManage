using org.huage.Entity.common;

namespace org.huage.Entity.Model;

public class SchedulerModel : BaseFiled
{
    public Guid Id { get; set; }

    /**
     * 方法名称
     */
    public string MethodName { get; set; }

    /**
     * 方法参数
     */
    public string MethodParams{ get; set; }

    /**
     * cron表达式
     */
    public string CronExpression{ get; set; }

    /**
     * 状态（1为启用（包含两个状态，分别为停用和删除）, 0为停用,2更新(需要重新构建job)）
     */
    public int JobStatus{ get; set; }

    /**
     * 开始时间
     */
    public long StartTime{ get; set; }

    /**
     * 请求路径
     */
    public string Url{ get; set; }
    
    /**
     * 请求类型（get,post）
     */
    public string RequestType{ get; set; }
}