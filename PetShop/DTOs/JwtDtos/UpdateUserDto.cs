using System.ComponentModel.DataAnnotations;

namespace PetShop.DTOs.JwtDtos;

public class UpdateUserDto
{
    [Required(ErrorMessage = "User name is required")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "New password is required")]
    public string? NewPassword { get; set; }
    [Required(ErrorMessage = "Tutor id is required")]
    public int TutorId { get; set; }
}
