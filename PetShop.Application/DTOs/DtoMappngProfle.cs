using AutoMapper;
using PetShop.DTOs.JwtDtos;
using PetShop.DTOs.PetDtos;
using PetShop.DTOs.TutorDtos;
using PetShop.Models;

namespace PetShop.DTOs;

public class DtoMappngProfle : Profile
{
    public DtoMappngProfle()
    {
        CreateMap<Tutor, ResponseTutorDto>().ForMember(dest => dest.Pets, opt => opt.MapFrom(src => src.Pets)).ReverseMap();

        CreateMap<Tutor, UpdateTutorDto>().ReverseMap();

        CreateMap<Pet, UpdatePetDto>().ReverseMap();

        CreateMap<Pet, ResponsePetDto>().ReverseMap();

        CreateMap<RegisterTutorDto, Tutor>().ReverseMap();

        CreateMap<RegisterPetDto, Pet>().ReverseMap();

        CreateMap<RegisterDto, ApplicationUser>().ReverseMap();

        CreateMap<ApplicationUser, ResponseInformationsDto>().ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TutorId, opt => opt.MapFrom(src => src.TutorId)).ForMember(dest => dest.Role, opt => opt.Ignore())
            .ReverseMap();
    }
}


