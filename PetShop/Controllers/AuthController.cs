using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs.JwtDtos;
using PetShop.Filters;
using PetShop.Models;
using PetShop.Service;
using PetShop.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PetShop.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserServices _userServices;
    private readonly IConfiguration _configuration;

    public AuthController(ITokenService tokenService, IConfiguration configuration, UserServices userServices)
    {
        _userServices = userServices;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("Register")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var userExists = await _userServices.FindByNameAsync(registerDto.UserName);
        var emailExists = await _userServices.FindByEmailAsync(registerDto.Email);

        if (userExists != null || emailExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = "User or email already exist" });
        }

        ApplicationUser user = new()
        {
            Email = registerDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerDto?.UserName
        };
        var result = await _userServices.CreateUserAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = "User creation failed" });
        }

        return Ok(new ResponseDto { Status = "Success", Message = "User created successfully" });
    }

    [HttpPost]
    [Route("Login")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userServices.FindByNameAsync(loginDto.UserName!);
        var password = await _userServices.CheckPasswordAsync(user, loginDto.Password!);

        if (user != null && password != null)
        {
            var userRoles = await _userServices.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValityInMinutes"], out int refreshTokenValityInMinutes);

            user.RefrshToken = refreshToken;
            user.RefrshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValityInMinutes);

            await _userServices.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }

    [HttpPost]
    [Route("Refresh-Token")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> RefrshToken(TokenDto tokenDto)
    {
        if (tokenDto is null) return BadRequest("Invalid client request");

        string? accessToken = tokenDto.AccessToken ?? throw new ArgumentNullException(nameof(tokenDto));
        string? refreshToken = tokenDto.RefreshToken ?? throw new ArgumentNullException(nameof(tokenDto));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal == null) return BadRequest("Invalid access token/refresh token");

        string username = principal.Identity.Name;

        var user = await _userServices .FindByNameAsync(username!);

        if (user == null || user.RefrshToken != refreshToken || user.RefrshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid access token/refres token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefrshToken = newRefreshToken;
        await _userServices.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [Route("AddToRole")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
        var user = await _userServices.FindByEmailAsync(email);

        if (user != null)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Success", Message = $"User{user.Email} added to the {roleName} role" });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto { Status = "Error", Message = $"Error: Unable to add user {user.Email} to the  {roleName} role" });
            }
        }
        return BadRequest(new { error = "Unable to find user" });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userServices.FindByNameAsync(username);

        if (user == null) return BadRequest("Invalid user name");

        user.RefrshToken = null;
        await _userServices.UpdateAsync(user);

        return NoContent();
    }
}
