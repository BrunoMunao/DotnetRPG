using System.Security.Claims;
using dotnetRPG.DTOs.Character;
using dotnetRPG.Models;
using dotnetRPG.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnetRPG.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
        {
            var response = await _characterService.GetCharacterById(id);
            
            if(response.Data is null)
                return NotFound(response);
           
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<AddCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(AddCharacterDto updatedCharacter, int id)
        {
            var response = await _characterService.UpdateCharacter(updatedCharacter, id);

            if(response.Data is null)
                return NotFound(response);
           
            return Ok(response);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var response = await _characterService.DeleteCharacter(id);

            if(response.Data is null)
                return NotFound(response);
           
            return Ok(response);

        }
    }
}