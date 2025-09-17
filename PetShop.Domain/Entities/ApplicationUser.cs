using Microsoft.AspNetCore.Identity;

namespace PetShop.Models;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public int? TutorId { get; set; }
    public Tutor? Tutor { get; set; }
}