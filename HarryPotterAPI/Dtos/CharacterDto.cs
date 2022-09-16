using System.Text.Json.Serialization;
namespace HarryPotterAPI.Dtos
{
    public class CharacterDto
    {
        public string Name { get; set; }
        public string Link { get; set; }
        [JsonPropertyName("Descr")]
        public string Description { get; set; }
        public string Gender { get; set; }
        public string SpeciesRace { get; set; }
        public string Blood { get; set; }
        public string School { get; set; }
        public string Profession { get; set; }
    }
}
