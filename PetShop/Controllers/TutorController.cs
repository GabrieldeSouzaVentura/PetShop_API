using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.DTOs;
using PetShop.DTOs.TutorDtos;
using PetShop.Filters;
using PetShop.Models;
using PetShop.Repositories.IRepositories;
using PetShop.Service.IService;

namespace PetShop.Controllers;

[Route("Api/[controller]")]
[ApiController]

public class TutorController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserServices _userServices;
    private readonly IMapper _mapper;

    public TutorController(IUnitOfWork unitOfWork, IUserServices userServices, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userServices = userServices;
        _mapper = mapper;
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [Route("RegisterTutor")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<Tutor>> RegisterTutor([FromBody] RegisterTutorDto registerTutorDto)
    {
        if (registerTutorDto == null) return BadRequest("Tutor is invalid");

        var tutor = _mapper.Map<Tutor>(registerTutorDto);

        var creatTutor = _unitOfWork.TutorRepository.Create(tutor);
        await _unitOfWork.CommitAsync();

        var newTutor = _mapper.Map<ResponseTutorDto>(creatTutor);

        return Ok(newTutor);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    [Route("GetByNameTutor/{name}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<ResponseTutorDto>> GetByNameTutor(string name)
    {
        var result = await _unitOfWork.TutorRepository.GetAsync(n => n.Name == name, i => i.Include(p => p.Pets));

        if (result is null) return NotFound("Name not found");

        var tutorDto = _mapper.Map<ResponseTutorDto>(result);
        return Ok(tutorDto);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetAllInformationsTutors")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<IEnumerable<ResponseTutorDto>>> GetAllInformationsTutors()
    {
        var filter = await _userServices.GetFilterByUserAsync<Tutor, int>(p => p.TutorId);

        var tutors = await _unitOfWork.TutorRepository.GetAllAsync(filter!, t => t.Include(i => i.Pets));

        if (tutors is null || !tutors.Any()) return NotFound("There are no registered tutors");

        var tutorsDto = _mapper.Map<IEnumerable<ResponseTutorDto>>(tutors);

        return Ok(tutorsDto);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    [Route("UpdateTutor/{id}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<UpdateTutorDto>> UpdateTutor(int id, UpdateTutorDto updateTutorDto)
    {
        var user = await _unitOfWork.TutorRepository.GetAsync(t => t.TutorId == id);
        if (user == null) return NotFound("Tutor not found");

        var tutor = _mapper.Map(updateTutorDto, user);

        var tutorUpdate = _unitOfWork.TutorRepository.Update(tutor);
        await _unitOfWork.CommitAsync();

        var tutorUpdateDto = _mapper.Map<UpdateTutorDto>(tutorUpdate);

        return Ok(tutorUpdateDto);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete]
    [Route("DeleteTutor/{id}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult> DeleteTutor(int id)
    {
        var tutor = await _unitOfWork.TutorRepository.GetAsync(t => t.TutorId == id, i => i.Include(p => p.Pets));
        if (tutor == null) return NotFound("Tutor not found");

        if (tutor.Pets != null && tutor.Pets.Any()) { _unitOfWork.PetRepository.DeleteRange(tutor.Pets); }

        _unitOfWork.TutorRepository.Delete(tutor);

        await _unitOfWork.CommitAsync();

        var result = await _userServices.DeleteUserAsync(id);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok("Tutor, pets and user successfully deleted");
    }
}
