using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetShop.Application.Service.IService;
using PetShop.DTOs.JwtDtos;
using PetShop.Models;
using PetShop.PetShop.Application.Service.IService;

namespace PetShop.PetShop.Application.Service;

public class UserServices : IUserServices
{
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IIdentityServices _identityServices;

    public UserServices(
        IMapper mapper, IUserContextService userContextService, IIdentityServices identityServices)
    {
        _mapper = mapper;
        _userContextService = userContextService;
        _identityServices = identityServices;
    }

    public async Task<IEnumerable<ResponseInformationsDto>> GetUserInformationsAsync()
    {
        var user = await _userContextService.GetLoggerUserAsync();

        var isAdmin = await _identityServices.IsInRoleAsync(user);

        var users = await _identityServices.GetUsersByAuthorizationAsync(user, isAdmin);

        var dtoList = _mapper.Map<IEnumerable<ResponseInformationsDto>>(users);

        foreach (var dto in dtoList)
        {
            var appUser = users.First(u => u.Id == dto.UserId);
            var roles = await _identityServices.GetRolesAsync(appUser);
            dto.Role = roles.FirstOrDefault();
        }

        return dtoList;
    }

    public async Task<ResponseInformationsDto> GetByNameUser(string name)
    {
        var user = await _identityServices.FindByNameAsync(name);

        var dto = _mapper.Map<ResponseInformationsDto>(user);

        var role = await _identityServices.GetRolesAsync(user!);
        dto.Role = role.FirstOrDefault();

        return dto;
    }
}
