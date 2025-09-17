using System.ComponentModel.DataAnnotations;

namespace PetShop.DTOs.PetDtos;

public class UpdatePetDto
{
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Age is required")]
    public int Age { get; set; }
    [Required(ErrorMessage = "Type is required")]
    public string? Type { get; set; }
    [Required(ErrorMessage = "Breed is required")]
    public string? Breed { get; set; }
    [Required(ErrorMessage = "Tutor id is required")]
    public int TutorId { get; set; }
}
