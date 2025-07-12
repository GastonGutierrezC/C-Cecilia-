
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;

namespace API.Profiles;

public class InputUserProfile : Profile
{
    public InputUserProfile()
    {
        CreateMap<InputUser, InputUserResponse>();
        CreateMap<CreateInputUser, InputUser>();
        CreateMap<UpdateInputUser, InputUser>()
            .ForMember(dest => dest.InputId, opt => opt.MapFrom(src => src.InputId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

    }
}
