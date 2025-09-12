namespace PetShop.DTOs.PetDtos;

public class ResponsePetDto
{
    public int PetId { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Type { get; set; }
    public string? Breed { get; set; }
    public int TutorId { get; set; }
}
