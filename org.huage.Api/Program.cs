using NLog.Web;
using org.huage.Api.consul;
using org.huage.Api.Extension;
using org.huage.BizManagement.Job;
using org.huage.BizManagement.Manager;
using org.huage.BizManagement.MapProfile;
using org.huage.BizManagement.Redis;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureMysql(builder.Configuration);
builder.Services.ConfigureRedis(builder.Configuration);
builder.Services.ConfigureRepositoryWrapper();

builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(SchedulerMapProfile).Assembly);
builder.Services.AddTransient<ISchedulerManager, SchedulerManager>();
builder.Services.AddTransient<IRedisHelper, RedisHelper>();
builder.Services.AddSingleton<IJobFactory, DefaultScheduleServiceFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
builder.Services.AddSingleton<GetJob>();
builder.Services.AddSingleton<PostJob>();
builder.Services.AddTransient<IQuartzManage, QuartzManage>();
builder.Logging.AddNLog("config/NLog.config");
builder.Services.Configure<ConsulOption>(builder.Configuration.GetSection("ConsulOption"));
builder.Services.AddHostedService<RestartRegisterScheduler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseConsulRegistry(app.Lifetime);

app.MapGet("/api/health", () =>
{
    global::System.Console.WriteLine("Ok");
    return new
    {
        Message = "Ok"
    };
});
app.UseHttpsRedirection();
app.UseCors("AnyPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();