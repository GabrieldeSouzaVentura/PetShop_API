using System.Text.Json.Serialization;

namespace PetShop.Models;

public class Pet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Type { get; set; }
    public string Brees { get; set; }
    public int TutorId { get; set; }
    [JsonIgnore]
    public Tutor Tutor { get; set; }
}
