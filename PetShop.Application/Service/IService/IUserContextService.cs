using PetShop.Models;

namespace PetShop.Application.Service.IService;

public interface IUserContextService
{
    Task<ApplicationUser> GetLoggerUserAsync();
}
