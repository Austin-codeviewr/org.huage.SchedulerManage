namespace org.huage.BizManagement.Proxy;

public static class SchedulerUtils
{
    
    public static List<int> GetTimeUnit(long startTime)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(startTime);
        var dateTime = dateTimeOffset.LocalDateTime;
        //如果设置的开始时间比现在时间小，在设置为当前时间。
        if (DateTime.Compare(DateTime.Now,dateTime) > 0)
        {
            dateTime = DateTime.Now.Add(TimeSpan.FromSeconds(5));
        }
        int year = dateTime.Year;
        int month = dateTime.Month;
        int day = dateTime.Day;
        int hour = dateTime.Hour;
        int minute = dateTime.Minute;
        int second = dateTime.Second;
        
        return new List<int>()
        {
            year,month,day,hour,minute,second
        };

    }
}