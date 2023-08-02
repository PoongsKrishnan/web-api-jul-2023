using AutoMapper;
using EmployeesHrApi.Data;
using EmployeesHrApi.Models;


namespace EmployeesHrApi.AutomapperProfiles;



public class HiringRequestProfile : Profile
{
    public HiringRequestProfile()
    {
        CreateMap<HiringRequestCreateRequest, HiringRequests>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => HiringRequestStatus.WaitingForJobAssignment));



        CreateMap<HiringRequests, HiringRequestResponseModel>();
    }
}