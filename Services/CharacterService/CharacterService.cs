using System.Security.Claims;
using AutoMapper;
using dotnetRPG.Data;
using dotnetRPG.DTOs.Character;
using dotnetRPG.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetRPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            var dbCharacter = _mapper.Map<Character>(newCharacter);
            dbCharacter.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            _context.Characters.Add(dbCharacter);
            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
                if (dbCharacter is null)
                    throw new Exception($"Character with Id '{id}' not found.");

                _context.Characters.Remove(dbCharacter);
                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters
                    .Where(c => c.User!.Id == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.Where(c => c.User!.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var dbCharacter = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
                if (dbCharacter is null)
                    throw new Exception($"Character with Id '{id}' not found.");
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(AddCharacterDto updatedCharacter, int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {

                var dbCharacter = await _context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (dbCharacter is null || dbCharacter.User!.Id != GetUserId())
                    throw new Exception($"Character with Id '{id}' not found.");

                dbCharacter.Name = updatedCharacter.Name;
                dbCharacter.HitPoints = updatedCharacter.HitPoints;
                dbCharacter.Strength = updatedCharacter.Strength;
                dbCharacter.Defense = updatedCharacter.Defense;
                dbCharacter.Intelligence = updatedCharacter.Intelligence;
                dbCharacter.Class = updatedCharacter.Class;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}