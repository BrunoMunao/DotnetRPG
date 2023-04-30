using AutoMapper;
using dotnetRPG.DTOs.Character;
using dotnetRPG.Models;

namespace dotnetRPG
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
        }
    }
}