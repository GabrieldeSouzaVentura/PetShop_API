using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetShop.DTOs.JwtDtos;
using PetShop.Filters;
using PetShop.Models;
using PetShop.Service.IService;

namespace PetShop.Controllers;

[Route("Api/Controller")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    [Route("Register")]
    [ServiceFilter(typeof(PetShopExceptionFilter))]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var userExists = await _userManager.FindByNameAsync(registerDto.UserName!);
        var emailExists = await _userManager.FindByEmailAsync(registerDto.Email);

        if (userExists != null || emailExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Staus = "Error", Massage = "User or email already exist" });
        }


    }

}
