
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;

namespace API.Profiles;

public class InputIngredientProfile : Profile
{
    public InputIngredientProfile()
    {
        CreateMap<InputIngredients, InputIngredientResponse>();
        CreateMap<CreateInputIngredient, InputIngredients>();
        CreateMap<UpdateInputIngredient, InputIngredients>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.InputId, opt => opt.MapFrom(src => src.InputId))
            .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.IngredientId));

    }
}
