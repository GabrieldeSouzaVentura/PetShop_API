using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetShop.Application.Service.Exceptions;
using PetShop.Application.Service.IService;
using PetShop.DTOs;
using PetShop.DTOs.TutorDtos;
using PetShop.Models;
using PetShop.PetShop.Application.Service.IService;
using PetShop.Repositories.IRepositories;

namespace PetShop.Application.Service;

public class TutorServises : ITutorServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserServices _userServices;

    public TutorServises(IUnitOfWork unitOfWork, IMapper mapper, IUserServices userServices)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userServices = userServices;
    }

    public async Task<bool> DeleteTutor(int id)
    {
        var tutor = await _unitOfWork.TutorRepository.GetAsync(t => t.TutorId == id, i => i.Include(p => p.Pets));

        if (tutor == null) throw new ResourceNotFoundException($"Tutor: {id} not found");

        if (tutor.Pets != null && tutor.Pets.Any()) { _unitOfWork.PetRepository.DeleteRange(tutor.Pets); }

        _unitOfWork.TutorRepository.Delete(tutor);

        await _unitOfWork.CommitAsync();

        var result = await _userServices.DeleteUserAsync(id);

        return result.Succeeded;
    }

    public async Task<IEnumerable<ResponseTutorDto>> GetAllInformationsTutor()
    {
        var filter = await _userServices.GetFilterByUserAsync<Tutor, int>(p => p.TutorId);

        var tutors = await _unitOfWork.TutorRepository.GetAllAsync(filter!, t => t.Include(i => i.Pets));

        if (tutors is null || !tutors.Any()) throw new ResourceNotFoundException("There are no registered tutors");

        var tutorsDto = _mapper.Map<IEnumerable<ResponseTutorDto>>(tutors);

        return tutorsDto;
    }

    public async Task<ResponseTutorDto> GetByNameTutor(string name)
    {
        var result = await _unitOfWork.TutorRepository.GetAsync(n => n.Name == name, i => i.Include(p => p.Pets));

        if (result is null) throw new ResourceNotFoundException($"Name: {name} not found");

        var tutorDto = _mapper.Map<ResponseTutorDto>(result);

        return tutorDto;
    }

    public async Task<ResponseTutorDto> RegisterTutor(RegisterTutorDto registerTutorDto)
    {
        var tutor = _mapper.Map<Tutor>(registerTutorDto);

        var creatTutor = _unitOfWork.TutorRepository.Create(tutor);

        await _unitOfWork.CommitAsync();

        var newTutor = _mapper.Map<ResponseTutorDto>(creatTutor);

        return newTutor;
    }

    public async Task<UpdateTutorDto> UpdateTutor(int id, UpdateTutorDto updateTutorDto)
    {
        var user = await _unitOfWork.TutorRepository.GetAsync(t => t.TutorId == id);

        if (user == null) throw new ResourceNotFoundException($"Tutor: {id} not found");

        var tutor = _mapper.Map(updateTutorDto, user);

        var tutorUpdate = _unitOfWork.TutorRepository.Update(tutor);

        await _unitOfWork.CommitAsync();

        var tutorUpdateDto = _mapper.Map<UpdateTutorDto>(tutorUpdate);

        return tutorUpdateDto;
    }
}
