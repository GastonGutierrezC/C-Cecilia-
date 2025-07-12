
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;

namespace API.Profiles;

public class OutputUserProfile : Profile
{
    public OutputUserProfile()
    {
        CreateMap<OutputUser, OutputUserResponse>();
        CreateMap<CreateOutputUser, OutputUser>();
        CreateMap<UpdateOutputUser, OutputUser>()
            .ForMember(dest => dest.OutputId, opt => opt.MapFrom(src => src.OutputId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

    }
}
