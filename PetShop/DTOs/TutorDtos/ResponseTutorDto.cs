using PetShop.DTOs.PetDtos;

namespace PetShop.DTOs;

public class ResponseTutorDto
{
    public int? TutorId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? ContactNumber { get; set; }
    public string? Address { get; set; }
    public List<ResponsePetDto> Pets { get; set; } = new();
}
