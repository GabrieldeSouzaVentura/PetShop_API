using PetShop.Models;

namespace PetShop.DTOs;

public class TutorDto
{
    public string Name { get; set; }
    public string NumberContect { get; set; }
    public string Address { get; set; }
    public List<Pet> PetsDto { get; set; } = new();
}
