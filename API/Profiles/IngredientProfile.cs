
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.Entities;

namespace API.Profiles;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<Ingredient, IngredientResponse>();
        CreateMap<CreateIngredient, Ingredient>();
        CreateMap<UpdateIngredient, Ingredient>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.IngredientUnit, opt => opt.MapFrom(src => src.IngredientUnit))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.SellPrice, opt => opt.MapFrom(src => src.SellPrice));
    }
}
