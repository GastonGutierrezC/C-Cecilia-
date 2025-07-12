// API/Profiles/InputProfile.cs
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;

namespace API.Profiles;

public class InputProfile : Profile
{
    public InputProfile()
    {
        CreateMap<Input, InputResponse>();
        CreateMap<CreateInput, Input>();
        CreateMap<UpdateInput, Input>()
            .ForMember(dest => dest.InputDate, opt => opt.MapFrom(src => src.InputDate));
    }
}
