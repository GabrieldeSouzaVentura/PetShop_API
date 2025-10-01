using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShop.Application.Service.IService;
using PetShop.DTOs.JwtDtos;
using PetShop.Models;
using PetShop.PetShop.Api.Filters;
using PetShop.PetShop.Application.Service.IService;
using PetShop.Repositories.IRepositories;

namespace PetShop.PetShop.Api.Controllers;

[Route("Api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthServices _authServices;

    public AuthController(IAuthServices authServices)
    {
        _authServices = authServices;
    }

    [HttpPost]
    [Route("Login")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var authResponse = await _authServices.LoginAsync(loginDto);

        if (authResponse == null) return Unauthorized(new { Message = "Invalid credentials or missing required information" });

        return Ok(authResponse);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [Route("RegisterUser")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _authServices.RegisterUserAsync(registerDto);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
            {
                Status = "Error",
                Message = result.Errors.FirstOrDefault()?.Description ?? "User creation failed"
            });
        }

        return Ok(new ResponseDto { Status = "Success", Message = "User created successfuly" });
    }

    [HttpPost]
    [Route("Refresh-Token")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> RefrshToken([FromBody]TokenDto tokenDto)
    {
        if (tokenDto is null) return BadRequest("Invalid client request");

        string? accessToken = tokenDto.AccessToken ?? throw new ArgumentNullException(nameof(tokenDto));
        string? refreshToken = tokenDto.RefreshToken ?? throw new ArgumentNullException(nameof(tokenDto));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal == null) return BadRequest("Invalid access token/refresh token");

        string username = principal.Identity!.Name!;

        var user = await _userServices .FindByNameAsync(username!);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid access token/refres token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userServices.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [Route("AddUserToRole")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
        var result = await _userServices.AddToRoleAsync(email, roleName);

        if (result != null)
        {
            return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Success", Message = $"User {email} added to the {roleName} role" });
        }
        else
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto { Status = "Error", Message = $"Error: Unable to add user {email} to the  {roleName} role" });
        }
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [Route("GetUserInformations")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<ResponseInformationsDto>> GetUserInformations()
    {
        var user = await _userServices.GetUserInformationsAsync();

        return Ok(user);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    [Route("GetByNameUser/{name}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<ActionResult<ResponseInformationsDto>> GetByNameUser(string name)
    {
        var user = await _userServices.GetByNameUser(name);

        if (user == null) return NotFound("Name not found");

        return Ok(user);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut]
    [Route("UpdateUser/{id}")]
    [ServiceFilter(typeof (PetShopExceptionFilter))]
    public async Task<ActionResult<UpdateUserDto>> UpdateUserAsync(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        var result = await _userServices.UpdateUser(id, updateUserDto);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete]
    [Route("DeleteUser/{id}")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userServices.DeleteUserAsync(id);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(result);
    }
}
