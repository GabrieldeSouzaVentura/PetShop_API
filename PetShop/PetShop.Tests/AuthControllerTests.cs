using Microsoft.AspNetCore.Identity;
using Moq;
using PetShop.Models;
using PetShop.Service;

namespace PetShop.PetShop.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly UserServices _userServicesMock;

        public UserServicesTests()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object, null);

            _userServicesMock = new UserServices(_userManagerMock.Object);
        }
    }
}
