namespace org.huage.Entity.common;

public static class SchedulerKeyGenerator
{
    private static string _prefix = string.Empty;
    public static void SetPrefix(string prefix) => _prefix = !string.IsNullOrWhiteSpace(prefix) ? $"{prefix}:" : string.Empty;
    
    public static string TriggerKey(object id) => $"{_prefix}Trigger:{id}";
    public static string GroupKey(object id) => $"{_prefix}Group:{id}";
    public static string JobKey(object id) => $"{_prefix}Job:{id}";

}