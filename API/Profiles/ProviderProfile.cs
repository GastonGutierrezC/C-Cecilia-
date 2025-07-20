using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;

namespace API.Profiles;

public class ProviderProfile: Profile
{
    public ProviderProfile()
    {
        CreateMap<Provider, ProviderResponse>();
        CreateMap<CreateProvider, Provider>();
        CreateMap<UpdateProvider, Provider>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ObjetiveMount, opt => opt.MapFrom(src => src.ObjetiveMount));

    }
}