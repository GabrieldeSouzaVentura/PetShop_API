using PetShop.DTOs.PetDtos;

namespace PetShop.Application.Service.IService;

public interface IPetServices
{
    Task<ResponsePetDto> RegisterPet(RegisterPetDto registerPetDto);
    Task<ResponsePetDto> GetByNamePet(string name);
    Task<IEnumerable<ResponsePetDto>> GetAllPets();
    Task<UpdatePetDto> UpdatePet(int id, UpdatePetDto updatePetDto);
    Task<bool> DeletePet(int id);
}
