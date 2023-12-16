using Consul;
using Microsoft.Extensions.Options;

namespace org.huage.Api.consul;

public class ConsulBalancer
{
    private ConsulOption _consulOptions;

    public ConsulBalancer(IOptionsMonitor<ConsulOption> options)
    {
        _consulOptions = options.CurrentValue;
        Console.WriteLine("触发更新:" + _consulOptions.Port);
    }

    /// <summary>
    /// 根据服务名称随机获取一个对应的威微服务实例
    /// </summary>
    /// <param name="serviceName">微服务名称</param>
    /// <returns></returns>
    public AgentService ChooseService(string serviceName)
    {
        var consulClient = new ConsulClient(c => c.Address = new Uri(_consulOptions.ConsulHost!));
        var services = consulClient.Agent.Services().Result.Response;
        var targetServices = services.Where(c => c.Value.Service.Equals(serviceName)).Select(c => c.Value);
        var targetService = targetServices!.ElementAt(new Random().Next(1, 1000) % targetServices.Count());

        return targetService;
    }
}