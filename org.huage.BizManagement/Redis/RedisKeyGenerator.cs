namespace org.huage.BizManagement.Redis;

public static class RedisKeyGenerator
{
    private static string _prefix = string.Empty;
    public static void SetPrefix(string prefix) => _prefix = !string.IsNullOrWhiteSpace(prefix) ? $"{prefix}:" : string.Empty;
    
    public static string AllSchedulersRedisKey() => $"{_prefix}AllSchedulers";

}