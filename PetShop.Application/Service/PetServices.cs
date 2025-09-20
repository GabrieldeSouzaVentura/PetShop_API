using AutoMapper;
using PetShop.Application.Service.IService;
using PetShop.DTOs.PetDtos;
using PetShop.Models;
using PetShop.PetShop.Application.Service.IService;
using PetShop.Repositories.IRepositories;

namespace PetShop.Application.Service;

public class PetServices : IPetServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserServices _userServices;

    public PetServices(IUnitOfWork unitOfWork, IMapper mapper, IUserServices userServices)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userServices = userServices;
    }

    public async Task<bool> DeletePet(int id)
    {
        var pet = await _unitOfWork.PetRepository.GetAsync(p => p.PetId == id);

        if (pet == null) throw new Exception("Pet not found");

        _unitOfWork.PetRepository.Delete(pet);

        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<IEnumerable<ResponsePetDto>> GetAllPets()
    {
        var filter = await _userServices.GetFilterByUserAsync<Pet, int>(t => t.TutorId);

        var pets = await _unitOfWork.PetRepository.GetAllAsync(filter!);

        if (pets is null || !pets.Any()) throw new Exception("Pets not found");

        var petsDto = _mapper.Map<IEnumerable<ResponsePetDto>>(pets);

        return petsDto;
    }

    public Task<ResponsePetDto> GetByNamePet(string name)
    {
        throw new NotImplementedException();
    }

    public Task<ResponsePetDto> RegisterPet(RegisterPetDto registerPetDto)
    {
        throw new NotImplementedException();
    }

    public Task<UpdatePetDto> UpdatePet(int id, UpdatePetDto updatePetDto)
    {
        throw new NotImplementedException();
    }
}
