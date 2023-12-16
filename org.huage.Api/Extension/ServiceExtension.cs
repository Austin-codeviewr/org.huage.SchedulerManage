using Consul;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using org.huage.Api.consul;
using org.huage.BizManagement.Wrapper;
using org.huage.BizManagement.Wrapper.impl;
using org.huage.EntityFramewok.Database.DBContext;
using StackExchange.Redis;

namespace org.huage.Api.Extension;

public static class ServiceExtension
{
    public static void ConfigureCors(this IServiceCollection service)
    {
        service.AddCors(option =>
        {
            option.AddPolicy("AnyPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
    }
    
    public static void ConfigureMysql(this IServiceCollection service,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("GameDb");
        service.AddDbContext<SchedulerDbContext>(builder =>
            builder.UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion));
    }
    
    public static void ConfigureRedis(this IServiceCollection service,IConfiguration configuration)
    {
        service.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            var connectionString = configuration.GetConnectionString("redis");
            return ConnectionMultiplexer.Connect(connectionString);
        });
    }
    
    public static WebApplication  UseConsulRegistry(this WebApplication webApplication, IHostApplicationLifetime builder)
    {
        var optionsMonitor = webApplication.Services.GetService<IOptionsMonitor<ConsulOption>>();
        var consulOption = optionsMonitor!.CurrentValue;
        //获取心跳监测的Ip和port
        var consulOptionIp = webApplication.Configuration["ip"] ?? consulOption.IP;
        var port = webApplication.Configuration["Port"] ?? consulOption.Port;
        
        //生成serviceId
        var id = Guid.NewGuid().ToString();
        
        //创建连接Consul客户端对象
        var consulClient = new ConsulClient(c =>
        {
            c.Address = new Uri(consulOption.ConsulHost!);
            c.Datacenter = consulOption.ConsulDataCenter;
        });
        //把服务注册到consul上
        consulClient.Agent.ServiceRegister(new AgentServiceRegistration()
        {
            ID = id,
            Name = consulOption.ServiceName,
            Address = consulOptionIp,
            Port = Convert.ToInt32(port),
            Check = new AgentServiceCheck()
            {
                Interval = TimeSpan.FromSeconds(12),
                HTTP = $"http://{consulOptionIp}:{port}/api/health",
                Timeout = TimeSpan.FromSeconds(5),
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20)
            }
        });


        builder.ApplicationStopped.Register(async () =>
        {
            Console.WriteLine("服务注销");
            await consulClient.Agent.ServiceDeregister(id);
        });

        return webApplication;
    }
    
    public static void ConfigureRepositoryWrapper(this IServiceCollection service)
    {
        service.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }
}