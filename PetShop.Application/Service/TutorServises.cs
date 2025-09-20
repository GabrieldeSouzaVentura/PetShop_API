using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        if (tutor == null) throw new Exception("Tutor not found");

        if (tutor.Pets != null && tutor.Pets.Any()) { _unitOfWork.PetRepository.DeleteRange(tutor.Pets); }

        _unitOfWork.TutorRepository.Delete(tutor);

        await _unitOfWork.CommitAsync();

        var result = await _userServices.DeleteUserAsync(id);

        return result.Succeeded;
    }

    public Task<ResponseTutorDto> GetAllInformationsTutor()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseTutorDto> GetByNameTutor(string name)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseTutorDto> RegisterTutor(RegisterTutorDto registerTutorDto)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateTutorDto> UpdateTutor(int id, UpdateTutorDto updateTutorDto)
    {
        throw new NotImplementedException();
    }
}
