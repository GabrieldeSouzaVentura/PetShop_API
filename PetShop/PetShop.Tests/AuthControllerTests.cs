using Microsoft.AspNetCore.Identity;
using Moq;
using PetShop.DTOs.JwtDtos;
using PetShop.Models;
using PetShop.Service;

namespace PetShop.PetShop.Tests;

public class AuthControllerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserServices _userServices;

    public AuthControllerTests()
    {
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStore.Object, null, null, null, null, null, null, null, null
        );

        var roleStore = new Mock<IRoleStore<IdentityRole>>();
        var logger = new Mock<ILogger<RoleManager<IdentityRole>>>();
        var validators = new List<IRoleValidator<IdentityRole>>();
        var errorDescriber = new IdentityErrorDescriber();

        _roleManager = new RoleManager<IdentityRole>(
            roleStore.Object,
            validators,
            new UpperInvariantLookupNormalizer(),
            errorDescriber,
            logger.Object
        );

        _userServices = new UserServices(_userManagerMock.Object, _roleManager);
    }

    [Fact]
    public async Task RegisterUserValid()
    {
        var dto = new RegisterDto
        {
            UserName = "CreateUser",
            Email = "createUser@gmail.com",
            Password = "CreateUser.1"
        };

        _userManagerMock.Setup(u => u.FindByNameAsync(dto.UserName)).ReturnsAsync((ApplicationUser)null!);
        _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync((ApplicationUser)null!);
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password)).ReturnsAsync(IdentityResult.Success);

        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        var result = await _userServices.CreateUserAsync(user, dto.Password);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task RegisterUser_WhenCreationFails_ReturnsFailed()
    {
        var dto = new RegisterDto
        {
            UserName = "FailUser",
            Email = "failUser@gmail.com",
            Password = "FailUser.1"
        };

        _userManagerMock.Setup(u => u.FindByNameAsync(dto.UserName)).ReturnsAsync((ApplicationUser)null!);
        _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync((ApplicationUser)null!);
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Erro simulado" }));

        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        var result = await _userServices.CreateUserAsync(user, dto.Password);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task RegisterUser_WhenUserAlreadyExists_ReturnsFailed()
    {
        var dto = new RegisterDto
        {
            UserName = "UserExists",
            Email = "userExists@gmail.com",
            Password = "UserExists.1"
        };

        _userManagerMock.Setup(u => u.FindByNameAsync(dto.UserName)).ReturnsAsync(new ApplicationUser());
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Usuário já existe" }));

        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        var result = await _userServices.CreateUserAsync(user, dto.Password);

        Assert.False(result.Succeeded);
    }
}
