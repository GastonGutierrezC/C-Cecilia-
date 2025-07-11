// API/Profiles/OutputProductProfile.cs
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;

namespace API.Profiles;

public class OutputProductProfile : Profile
{
    public OutputProductProfile()
    {
        CreateMap<OutputProducts, OutputProductResponse>();
        CreateMap<CreateOutputProduct, OutputProducts>();
        CreateMap<UpdateOutputProduct, OutputProducts>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.OutputId, opt => opt.MapFrom(src => src.OutputId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

    }
}
