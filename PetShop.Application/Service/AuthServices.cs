using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PetShop.Application.Service.IService;
using PetShop.DTOs.JwtDtos;
using PetShop.Models;
using PetShop.PetShop.Application.Service;
using PetShop.PetShop.Application.Service.IService;
using PetShop.Repositories.IRepositories;

namespace PetShop.Application.Service;

public class AuthServices : IAuthServices
{
    private readonly IIdentityServices _identityServices;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthServices(IIdentityServices identityServices, ITokenService tokenService, IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _identityServices = identityServices;
        _tokenService = tokenService;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TokenDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _identityServices.FindByNameAsync(loginDto.UserName!);

        if (user == null || !await _identityServices.CheckPasswordAsync(user, loginDto.Password!))
            return null;

        var userRoles = await _identityServices.GetRolesAsync(user);

        var isAdmin = userRoles.Contains("Admin");

        if (user.TutorId == null && !isAdmin)
        {
            return null;
        }

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        if (user.TutorId != null) { authClaims.Add(new Claim("TutorId", user.TutorId.ToString()!)); }

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

        var refreshToken = _tokenService.GenerateRefreshToken();

        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

        await _identityServices.UpdateAsync(user);

        return new TokenDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken
        };
    }

    public async Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto)
    {
        var userExists = await _identityServices.FindByNameAsync(registerDto.UserName!);
        var emailExists = await _identityServices.ExistsByEmailAsync(registerDto.Email!);
        var tutorExists = await _unitOfWork.TutorRepository.GetAsync(t => t.TutorId == registerDto.TutorId!);
        var tutorIdExists = await _identityServices.ExistsByTutorIdAsync(registerDto.TutorId);

        if (userExists != null || emailExists != false || tutorExists == null || tutorIdExists)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "User, tutor or email is invalid or already in use"
            });
        }

        var user = _mapper.Map<ApplicationUser>(registerDto);
        user.SecurityStamp = Guid.NewGuid().ToString();

        var result = await _identityServices.CreateUserAsync(user, registerDto.Password!);

        return result;
    }
}
