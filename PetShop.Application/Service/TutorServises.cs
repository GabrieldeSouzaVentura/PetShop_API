using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PetShop.Application.Service.IService;
using PetShop.DTOs;
using PetShop.DTOs.TutorDtos;
using PetShop.Repositories.IRepositories;

namespace PetShop.Application.Service;

public class TutorServises : ITutorServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public async Task Delete(int id)
    {
        var tutor = await _unitOfWork.TutorRepository.GetAsync(t => t.TutorId == id, i => i.Include(p => p.Pets));

        if (tutor == null) throw new Exception("Tutor not found");
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
