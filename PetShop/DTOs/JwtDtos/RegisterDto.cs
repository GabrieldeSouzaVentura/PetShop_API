using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PetShop.DTOs.JwtDtos;

public class RegisterDto
{
    [Required(ErrorMessage = "User name is required")]
    public string? UserName { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}
