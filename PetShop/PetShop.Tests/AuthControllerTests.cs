using Microsoft.AspNetCore.Identity;
using Moq;
using PetShop.DTOs.JwtDtos;
using PetShop.Models;
using PetShop.Service;

namespace PetShop.PetShop.Tests;

public class AuthControllerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly UserServices _userServices;

    public AuthControllerTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            store.Object, null, null, null, null, null, null, null, null
        );

        var role = new Mock<IRoleStore<IdentityRole>>();
        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            role.Object, null, null, null, null, null, null, null, null);

        _userServices = new UserServices(_userManagerMock.Object, _roleManagerMock.Object);
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
    public async Task ExistingUserRegistration()
    {
        var dto = new RegisterDto
        {
            UserName = "UserExists",
            Email = "userExists@gmail.com",
            Password = "UserExists.1"
        };

        _userManagerMock.Setup(u => u.FindByNameAsync(dto.UserName)).ReturnsAsync(new ApplicationUser());

        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        var result = await _userServices.CreateUserAsync(user, dto.Password);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task RegisterUserNull()
    {
        var dto = new RegisterDto
        {
            UserName = "UserNull",
            Email = "userNull@gmail.com",
            Password = "UserNull.1"
        };

        _userManagerMock.Setup(u => u.FindByNameAsync(dto.UserName)).ReturnsAsync((ApplicationUser)null!);

        _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync((ApplicationUser)null!);

        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error User is Null"}));
        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        var result = await _userServices.CreateUserAsync(user, dto.Password);

        Assert.False(result.Succeeded);
    }
}
