using System.Text.Json.Serialization;
using PetShop.Models;

namespace PetShop.Domain.Entities;

public class User
{
    [JsonIgnore]
    public int UserId { get; set; }
    public int TutorId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? Role { get; set; }
    [JsonIgnore]
    public Tutor Tutor { get; set; }
}
