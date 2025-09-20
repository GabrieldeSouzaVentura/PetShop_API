using PetShop.DTOs;
using PetShop.DTOs.TutorDtos;

namespace PetShop.Application.Service.IService;

public interface ITutorServices
{
    Task<ResponseTutorDto> RegisterTutor(RegisterTutorDto registerTutorDto);
    Task<ResponseTutorDto> GetByNameTutor(string name);
    Task<ResponseTutorDto> GetAllInformationsTutor();
    Task<UpdateTutorDto> UpdateTutor(int id, UpdateTutorDto updateTutorDto);
    Task<bool> DeleteTutor(int id);
}
