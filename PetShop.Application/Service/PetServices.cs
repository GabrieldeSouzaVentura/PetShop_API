using AutoMapper;
using PetShop.Application.Service.Exceptions;
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

        if (pet == null) throw new ResourceNotFoundException($"Pet: {id} not found");

        _unitOfWork.PetRepository.Delete(pet);

        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<IEnumerable<ResponsePetDto>> GetAllPets()
    {
        var filter = await _userServices.GetFilterByUserAsync<Pet, int>(t => t.TutorId);

        var pets = await _unitOfWork.PetRepository.GetAllAsync(filter!);

        if (pets is null || !pets.Any()) throw new ResourceNotFoundException("Pets not found");

        var petsDto = _mapper.Map<IEnumerable<ResponsePetDto>>(pets);

        return petsDto;
    }

    public async Task<ResponsePetDto> GetByNamePet(string name)
    {
        var filter = await _userServices.GetFilterByNameAndUserAsync<Pet, int>(name, t => t.Name!, t => t.TutorId);

        var pets = await _unitOfWork.PetRepository.GetAsync(filter);

        if (pets == null) throw new ResourceNotFoundException($"Name pet: {name} not found");

        var petsDto = _mapper.Map<ResponsePetDto>(pets);

        return petsDto;
    }

    public async Task<ResponsePetDto> RegisterPet(RegisterPetDto registerPetDto)
    {
        var tutorExists = await _unitOfWork.TutorRepository.GetAsync(t => t.TutorId == registerPetDto.TutorId);

        if (tutorExists == null) throw new ResourceNotFoundException($"Tutor: {registerPetDto.TutorId} nor found");

        var pet = _mapper.Map<Pet>(registerPetDto);

        var petCreate = _unitOfWork.PetRepository.Create(pet);

        await _unitOfWork.CommitAsync();

        var newPet = _mapper.Map<ResponsePetDto>(petCreate);

        return newPet;
    }

    public async Task<UpdatePetDto> UpdatePet(int id, UpdatePetDto updatePetDto)
    {
        var petExists = await _unitOfWork.PetRepository.GetAsync(p => p.PetId == id);

        var tutorExists = await _unitOfWork.TutorRepository.GetAsync(t => t.TutorId == updatePetDto.TutorId);

        if (petExists == null || tutorExists == null) throw new ResourceNotFoundException("Pet or tutor not found");

        var pet = _mapper.Map(updatePetDto, petExists);

        var petUpdate = _unitOfWork.PetRepository.Update(pet);

        await _unitOfWork.CommitAsync();

        var petUpdateDto = _mapper.Map<UpdatePetDto>(petUpdate);

        return petUpdateDto;
    }
}
