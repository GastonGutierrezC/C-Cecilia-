// API/Profiles/OutputProfile.cs
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;

namespace API.Profiles;

public class OutputProfile : Profile
{
    public OutputProfile()
    {
        CreateMap<Output, OutputResponse>();
        CreateMap<CreateOutput, Output>();
        CreateMap<UpdateOutput, Output>()
         .ForMember(dest => dest.OutputDate, opt => opt.MapFrom(src => src.OutputDate))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

    }
}
