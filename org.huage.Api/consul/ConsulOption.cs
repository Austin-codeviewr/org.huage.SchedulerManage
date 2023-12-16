namespace org.huage.Api.consul;

public class ConsulOption
{
    public string? IP { get; set; }
    public string? Port { get; set; }
    /// <summary>
    /// 微服务名称
    /// </summary>
    public string? ServiceName { get; set; }
    
    /// <summary>
    /// consul对应的请求地址
    /// </summary>
    public string? ConsulHost { get; set; }
    
    /// <summary>
    /// consul数据中心,默认dc1
    /// </summary>
    public string?  ConsulDataCenter { get; set; }
}