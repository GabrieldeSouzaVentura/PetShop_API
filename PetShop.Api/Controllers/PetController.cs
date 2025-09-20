using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShop.Application.Service.IService;
using PetShop.DTOs.PetDtos;
using PetShop.Models;
using PetShop.PetShop.Api.Filters;
using PetShop.PetShop.Application.Service.IService;
using PetShop.Repositories.IRepositories;

namespace PetShop.PetShop.Api.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class PetController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserServices _userServices;
    private readonly IPetServices _petServices;

    public PetController(IUnitOfWork unitOfWork, IMapper mapper, IUserServices userServices, IPetServices petServices)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userServices = userServices;
        _petServices = petServices;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [Route("RegisterPet")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<Pet>> RegisterMyPet([FromBody] RegisterPetDto registerPetDto)
    {
        if (registerPetDto is null) return BadRequest("Pet invalid");

        var tutorExists = await _unitOfWork.TutorRepository.GetAsync(t => t.TutorId == registerPetDto.TutorId);

        if (tutorExists == null) return BadRequest("Tutor is invalid");

        var pet = _mapper.Map<Pet>(registerPetDto);

        var petCreate = _unitOfWork.PetRepository.Create(pet);
        await _unitOfWork.CommitAsync();

        var newPet = _mapper.Map<ResponsePetDto>(petCreate);

        return Ok(newPet);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetByNamePet/{name}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<ResponsePetDto>> GetByPetName(string name)
    {
        var filter = await _userServices.GetFilterByNameAndUserAsync<Pet, int>(name, t => t.Name!, t => t.TutorId);

        var pets = await _unitOfWork.PetRepository.GetAsync(filter);

        if (pets == null) return NotFound("Pets not found");

        var petsDto = _mapper.Map<ResponsePetDto>(pets);

        return Ok(petsDto);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetAllPets")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<IEnumerable<ResponsePetDto>>> GetAllPet()
    {
        var getAllPet = await _petServices.GetAllPets();

        return Ok(getAllPet);

        //var filter = await _userServices.GetFilterByUserAsync<Pet, int>(t => t.TutorId);

        //var pets = await _unitOfWork.PetRepository.GetAllAsync(filter!);

        //if (pets is null || !pets.Any()) return NotFound("Pets not found");

        //var petsDto = _mapper.Map<IEnumerable<ResponsePetDto>>(pets);

        //return Ok(petsDto);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    [Route("UpdatePet/{id}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<UpdatePetDto>> UpdatePet(int id, UpdatePetDto updatePetDto)
    {
        var user = await _unitOfWork.PetRepository.GetAsync(p => p.PetId == id);
        var tutorExists = await _unitOfWork.TutorRepository.GetAsync(t => t.TutorId == updatePetDto.TutorId);
        if (user == null || tutorExists == null) return NotFound("Pet or tutor not found");

        var pet = _mapper.Map(updatePetDto, user);

        var petUpdate = _unitOfWork.PetRepository.Update(pet);
        await _unitOfWork.CommitAsync();

        var petUpdateDto = _mapper.Map<UpdatePetDto>(petUpdate);

        return Ok(petUpdateDto);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete]
    [Route("DeletePet/{id}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> DeletePet(int id)
    {
        var result = await _petServices.DeletePet(id);

        if (result == true) return BadRequest("Error delete pet");

        return Ok("Pets successfully deleted");
    }
}