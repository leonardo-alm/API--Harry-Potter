using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HarryPotterAPI.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        [JsonPropertyName("Descr")]
        public string Description { get; set; }
        public string Gender { get; set; }
        public string SpeciesRace { get; set; }
        public string Blood { get; set; }
        public string School { get; set; }
        public string Profession { get; set; }

        public Character(int id, string name, string link, string description, string gender, string speciesRace, string blood, string school, string profession)
        {
            Id = id;
            Name = name;
            Link = link;
            Description = description;
            Gender = gender;
            SpeciesRace = speciesRace;
            Blood = blood;
            School = school;
            Profession = profession;
        }

        public Character clone()
        {
            return (Character)this.MemberwiseClone(); // Shallow Clone
        }
    }
}
