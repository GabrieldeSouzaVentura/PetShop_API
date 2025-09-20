using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShop.Application.Service.IService;
using PetShop.DTOs;
using PetShop.DTOs.TutorDtos;
using PetShop.Models;
using PetShop.PetShop.Api.Filters;
using PetShop.PetShop.Application.Service.IService;
using PetShop.Repositories.IRepositories;

namespace PetShop.PetShop.Api.Controllers;

[Route("Api/[controller]")]
[ApiController]

public class TutorController : ControllerBase
{
    private readonly ITutorServices _tutorServices;

    public TutorController(ITutorServices tutorServices)
    {
        _tutorServices = tutorServices;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [Route("RegisterTutor")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<Tutor>> RegisterTutor([FromBody] RegisterTutorDto registerTutorDto)
    {
        if (registerTutorDto == null) return BadRequest("Tutor is null");

        var registerTutor = await _tutorServices.RegisterTutor(registerTutorDto);

        return Ok(registerTutor);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    [Route("GetByNameTutor/{name}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<ResponseTutorDto>> GetByNameTutor(string name)
    {
        if (name == null) return BadRequest("Name is null");

        var getByNameTutor = await _tutorServices.GetByNameTutor(name);

        return Ok(getByNameTutor);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetAllInformationsTutors")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<IEnumerable<ResponseTutorDto>>> GetAllInformationsTutors()
    {
        var tutor = await _tutorServices.GetAllInformationsTutor();

        if (tutor is null) return NotFound("Tutor not found");

        return Ok(tutor);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    [Route("UpdateTutor/{id}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<UpdateTutorDto>> UpdateTutor(int id, UpdateTutorDto updateTutorDto)
    {
        var updateTutor = await _tutorServices.UpdateTutor(id, updateTutorDto);

        return Ok(updateTutorDto);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete]
    [Route("DeleteTutor/{id}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> DeleteTutor(int id)
    {
        var result = await _tutorServices.DeleteTutor(id);

        if (result == false) return BadRequest("Error delete tutor");

        return Ok("Tutor, pets and user successfully deleted");
    }
}
