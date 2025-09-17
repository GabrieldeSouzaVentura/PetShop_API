using System.Text.Json.Serialization;

namespace PetShop.DTOs.JwtDtos;

public class ResponseInformationsDto
{
    [JsonIgnore]
    public string? UserId { get; set; }
    public int TutorId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
}
