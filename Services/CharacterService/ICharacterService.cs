using dotnetRPG.DTOs.Character;
using dotnetRPG.Models;

namespace dotnetRPG.Services.CharacterService
{
    public interface ICharacterService
    {
        public Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();
        public Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);
        public Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
        public Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(AddCharacterDto updatedCharacter, int id);
        public Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id);
    }
}