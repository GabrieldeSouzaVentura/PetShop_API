using Microsoft.AspNetCore.Identity;
using Moq;
using PetShop.DTOs.JwtDtos;
using PetShop.Models;
using PetShop.Service;
using Xunit;

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
            UserName = "Gsventura",
            Email = "gsventura@gmail.com",
            Password = "Gsventura.1"
        };

        _userManagerMock.Setup(u => u.FindByNameAsync(dto.UserName)).ReturnsAsync((ApplicationUser)null!);

        _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email)).ReturnsAsync((ApplicationUser)null!);

        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), dto.Password)).ReturnsAsync(IdentityResult.Success);
        }
}
