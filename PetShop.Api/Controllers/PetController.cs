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
        if (registerPetDto is null) return BadRequest("Inconsistent pet data");

        var registerPet = await _petServices.RegisterPet(registerPetDto);

        return Ok(registerPet);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetByNamePet/{name}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<ResponsePetDto>> GetByPetName(string name)
    {
        if (name is null) return BadRequest("Name is required");

        var getByNamePet = await _petServices.GetByNamePet(name);

        return Ok(getByNamePet);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetAllPets")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<IEnumerable<ResponsePetDto>>> GetAllPet()
    {
        var getAllPet = await _petServices.GetAllPets();

        return Ok(getAllPet);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    [Route("UpdatePet/{id}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<UpdatePetDto>> UpdatePet(int id, UpdatePetDto updatePetDto)
    {
        if (updatePetDto is null) return BadRequest("Inconsistent pet data");

        var updatePet = await _petServices.UpdatePet(id,updatePetDto);

        return Ok(updatePet);
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