namespace org.huage.EntityFramewok.Database.Table;

public class Scheduler : BaseTable
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
     * 状态（1为启用, 0为停用）
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