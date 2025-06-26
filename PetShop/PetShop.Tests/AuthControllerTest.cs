using Moq;
using PetShop.Models;

namespace PetShop.PetShop.Tests;

public class AuthControllerTest
{
    public void TestRegister()
    {
        var mockRepository = new Mock<ApplicationUser>();
        var services = new Auth(mockRepository.Object);
    }
}
