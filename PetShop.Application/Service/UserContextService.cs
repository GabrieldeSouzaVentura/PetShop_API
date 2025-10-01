using Microsoft.AspNetCore.Identity;
using PetShop.Application.Service.IService;
using PetShop.Models;

namespace PetShop.Application.Service;

public class UserContextService : IUserContextService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public UserContextService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApplicationUser> GetLoggerUserAsync()
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);

        if (user == null) throw new UnauthorizedAccessException("User not found");

        return user;
    }
}
