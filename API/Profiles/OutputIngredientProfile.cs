// API/Profiles/OutputIngredientProfile.cs
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;

namespace API.Profiles;

public class OutputIngredientProfile : Profile
{
    public OutputIngredientProfile()
    {
        CreateMap<OutputIngredients, OutputIngredientResponse>();
        CreateMap<CreateOutputIngredient, OutputIngredients>();
        CreateMap<UpdateOutputIngredient, OutputIngredients>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.IngredientId))
            .ForMember(dest => dest.OutputId, opt => opt.MapFrom(src => src.OutputId));

    }
}
