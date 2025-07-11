using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs; 
using Core.Entities;

namespace API.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>();
        CreateMap<CreateUser, User>();
        CreateMap<UpdateUser, User>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
    }
}
