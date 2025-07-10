using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.Entities;

namespace API.Profiles;

public class ProductProfile: Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResponse>();
        CreateMap<CreateProduct, Product>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.InPrice, opt => opt.MapFrom(src => src.InPrice));
    }
}