using Microsoft.AspNetCore.Identity;
using PetShop.DTOs.JwtDtos;

namespace PetShop.Application.Service.IService;

public interface IAuthServices
{
    Task<TokenDto?> LoginAsync(LoginDto loginDto);
    Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto);
    Task<TokenDto?> RefreshTokenAsync(TokenDto tokenDto);
}
