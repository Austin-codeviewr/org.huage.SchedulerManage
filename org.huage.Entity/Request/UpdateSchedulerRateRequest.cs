namespace org.huage.Entity.Request;

public class UpdateSchedulerRateRequest 
{
    public Guid Id { get; set; }
    
    /**
     * cron表达式
     */
    public string CronExpression{ get; set; }
    
    /**
     * 开始时间
     */
    public long StartTime{ get; set; }
    
    public string Remark { get; set; }
    
    public string UpdateBy { get; set; }
    
    public DateTime UpdateTime { get; set; } =DateTime.Now;
    
}