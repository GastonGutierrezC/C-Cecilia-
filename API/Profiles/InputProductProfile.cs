// API/Profiles/InputProductProfile.cs
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;

namespace API.Profiles;

public class InputProductProfile : Profile
{
    public InputProductProfile()
    {
        CreateMap<InputProducts, InputProductResponse>();
        CreateMap<CreateInputProduct, InputProducts>();
        CreateMap<UpdateInputProduct, InputProducts>()
           .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.InputId, opt => opt.MapFrom(src => src.InputId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
    }
}
