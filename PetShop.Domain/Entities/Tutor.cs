using System.Text.Json.Serialization;

namespace PetShop.Models;

public class Tutor
{
    public int TutorId { get; set; }
    public string? RoleName { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? UserName { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    [JsonIgnore]
    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}