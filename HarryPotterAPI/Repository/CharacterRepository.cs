using HarryPotterAPI.Models;
using HarryPotterAPI.Interfaces;
using System.Text.Json;
using HarryPotterAPI.Dtos;
using System.IO;

namespace HarryPotterAPI.Repository
{
    public class CharacterRepository : ICharacterRepository
    {
        private List<Character> characters;

        public List<Character> GetCharacters(int page, int maxResults)
        {
            using var reader = new StreamReader("./hpData.json");
            string json = reader.ReadToEnd();
            characters = JsonSerializer.Deserialize<List<Character>>(json);

            characters = characters.Skip((page - 1) * maxResults).Take(maxResults).ToList();

            return characters;
        }

        public Character GetCharacterById(int id)
        {
            using var reader = new StreamReader("./hpData.json");
            string json = reader.ReadToEnd();
            characters = JsonSerializer.Deserialize<List<Character>>(json);

            var character = characters.Where(x => x.Id == id).FirstOrDefault();
            if (character == null) return null;

            return character;
        }

        public List<Character> GetStudent(string school, int page, int maxResults)
        {
            using var reader = new StreamReader("./hpData.json");
            string json = reader.ReadToEnd();
            characters = JsonSerializer.Deserialize<List<Character>>(json);

            characters = characters.Where(s => s.School.Contains(school, StringComparison.InvariantCultureIgnoreCase))
                                   .Skip((page - 1) * maxResults)
                                   .Take(maxResults)
                                   .ToList();

            if (!characters.Any()) return null;

            return characters;
        }

        public List<Character> GetCharactersByBody(CharacterDto characterDto, int page, int maxResults)
        {
            using var reader = new StreamReader("./hpData.json");
            string json = reader.ReadToEnd();
            characters = JsonSerializer.Deserialize<List<Character>>(json);

            if(!String.IsNullOrWhiteSpace(characterDto.Name) && characterDto.Name != "string")
            {
                characters = characters.Where(b => b.Name.Contains(characterDto.Name, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (!String.IsNullOrWhiteSpace(characterDto.Blood) && characterDto.Blood != "string")
            {
                characters = characters.Where(b => b.Blood.Contains(characterDto.Blood, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (!String.IsNullOrWhiteSpace(characterDto.Gender) && characterDto.Gender != "string")
            {
                characters = characters.Where(g => g.Gender.Contains(characterDto.Gender, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (!String.IsNullOrWhiteSpace(characterDto.SpeciesRace) && characterDto.SpeciesRace != "string")
            {
                characters = characters.Where(s => s.SpeciesRace.Contains(characterDto.SpeciesRace, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            if (!String.IsNullOrWhiteSpace(characterDto.School) && characterDto.School != "string")
            {
                characters = characters.Where(s => s.School.Contains(characterDto.School, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            if (!characters.Any()) return null;

            characters = characters
                .Skip((page - 1) * maxResults)
                .Take(maxResults)
                .ToList();

            return characters;
        }

        public Character InsertCharacter(CharacterDto newCharacter)
        {
            using var reader = new StreamReader("./hpData.json");
            string json = reader.ReadToEnd();
            reader.Dispose();
            characters = JsonSerializer.Deserialize<List<Character>>(json);

            var lastId = characters.OrderBy(x => x.Id).Last().Id + 1;
            var characterToInsert = new Character(lastId, newCharacter.Name, newCharacter.Link, newCharacter.Description, newCharacter.Gender, newCharacter.SpeciesRace, newCharacter.Blood, newCharacter.School, newCharacter.Profession);
            characters.Add(characterToInsert);

            var jsonCharacters = JsonSerializer.Serialize(characters);
            System.IO.File.WriteAllText("./hpData.json", jsonCharacters);

            return characterToInsert;
        }

        public Character RemoveCharacter(int id)
        {
            using var reader = new StreamReader("./hpData.json");
            string json = reader.ReadToEnd();
            reader.Dispose();
            characters = JsonSerializer.Deserialize<List<Character>>(json);

            var character = characters.Where(x => x.Id == id).FirstOrDefault();
            if (character == null) return null; 
            characters.Remove(character);

            var jsonCharacters = JsonSerializer.Serialize(characters);
            System.IO.File.WriteAllText("./hpData.json", jsonCharacters);

            return character;
        }

        public Character UpdateFullCharacter(int id, CharacterDto updatedCharacter)
        {
            using var reader = new StreamReader("./hpData.json");
            string json = reader.ReadToEnd();
            reader.Dispose();
            characters = JsonSerializer.Deserialize<List<Character>>(json);

            var character = characters.Where(x => x.Id == id).FirstOrDefault();
            if (character == null) return null; 

            character.School = updatedCharacter.School;
            character.Profession = updatedCharacter.Profession;
            character.Gender = updatedCharacter.Gender;
            character.Blood = updatedCharacter.Blood;
            character.Name = updatedCharacter.Name;
            character.Link = updatedCharacter.Link;
            character.SpeciesRace = updatedCharacter.SpeciesRace;

            var jsonCharacters = JsonSerializer.Serialize(characters);
            System.IO.File.WriteAllText("./hpData.json", jsonCharacters);

            return character;
        }

        public Character UpdateCharacterProfession(int id, CharacterProfessionDto updatedCharacter)
        {
            using var reader = new StreamReader("./hpData.json");
            string json = reader.ReadToEnd();
            reader.Dispose();
            characters = JsonSerializer.Deserialize<List<Character>>(json);

            var character = characters.Where(x => x.Id == id).FirstOrDefault();
            if (character == null) return null;

            character.Profession = updatedCharacter.Profession;

            var jsonCharacters = JsonSerializer.Serialize(characters);
            System.IO.File.WriteAllText("./hpData.json", jsonCharacters);

            return character;
        }
    }
}
