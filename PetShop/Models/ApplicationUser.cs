using Microsoft.AspNetCore.Identity;

namespace PetShop.Models;

public class ApplicationUser : IdentityUser
{
    public string? RefrshToken { get; set; }
    public DateTime RefrshTokenExpiryTime { get; set; }
    public int TutorId { get; set; }
}