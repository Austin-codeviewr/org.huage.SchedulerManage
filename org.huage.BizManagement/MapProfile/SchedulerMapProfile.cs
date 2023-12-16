using AutoMapper;
using org.huage.Entity.Model;
using org.huage.Entity.Request;
using org.huage.EntityFramewok.Database.Table;

namespace org.huage.BizManagement.MapProfile;

public class SchedulerMapProfile : Profile
{
    public SchedulerMapProfile()
    {
        CreateMap<AddSchedulerRequest, Scheduler>();
        CreateMap<Scheduler, SchedulerModel>();
        CreateMap<UpdateSchedulerRateRequest, Scheduler>();
        CreateMap<UpdateSchedulerRequest, Scheduler>();
        CreateMap<SchedulerModel, AddSchedulerRequest>();
    }
    
    
}