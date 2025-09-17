using System.ComponentModel.DataAnnotations;

namespace PetShop.DTOs.TutorDtos;

public class UpdateTutorDto
{
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Surname is required")]
    public string? Surname { get; set; }
    [Required(ErrorMessage = "Contect number is required")]
    public string? ContactNumber { get; set; }
    [Required(ErrorMessage = "Address is required")]
    public string? Address { get; set; }
}
