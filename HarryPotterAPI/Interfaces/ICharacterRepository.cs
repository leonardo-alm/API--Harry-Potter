using HarryPotterAPI.Dtos;
using HarryPotterAPI.Models;

namespace HarryPotterAPI.Interfaces
{
    public interface ICharacterRepository
    {
        public List<Character> GetCharacters(int page, int maxResults);
        public Character GetCharacterById(int id);
        public List<Character> GetStudent(string school, int page, int maxResults);
        public List<Character> GetCharactersByBody(CharacterDto characterDto, int page, int maxResults);
        public Character InsertCharacter(CharacterDto newCharacter);
        public Character RemoveCharacter(int id);
        public Character UpdateFullCharacter(int id, CharacterDto updatedCharacter);
        public Character UpdateCharacterProfession(int id, CharacterProfessionDto updatedCharacter);
    }
}
