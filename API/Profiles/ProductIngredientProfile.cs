// API/Profiles/ProductIngredientProfile.cs
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;

namespace API.Profiles;

public class ProductIngredientProfile : Profile
{
    public ProductIngredientProfile()
    {

        CreateMap<ProductIngredients, ProductIngredientResponse>();
        CreateMap<CreateProductIngredient, ProductIngredients>();
        CreateMap<UpdateProductIngredient, ProductIngredients>()
              .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.IngredientId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

    }
}
