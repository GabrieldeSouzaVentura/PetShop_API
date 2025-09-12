using System.ComponentModel.DataAnnotations;

namespace PetShop.DTOs.JwtDtos;

public class RegisterDto
{
    [Required(ErrorMessage = "User name is required")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "Emais is required")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
    [Required(ErrorMessage = "Tutor id is required")]
    public int? TutorId { get; set; }
}
