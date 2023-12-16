namespace org.huage.Entity.common;

public class SchedulerException : Exception
{
    public SchedulerException(Exception innerEx, string message)
        : base(message,innerEx)
    { }

    public SchedulerException(string message)
        : base(message)
    { }

    public const long UnknownErrCode = 00000001;
}