using Microsoft.AspNetCore.Identity;
using PetShop.Models;

namespace PetShop.Service;

public class UserServices
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityUser> _roleManager;

    public UserServices(UserManager<IdentityUser> userManager, RoleManager<IdentityUser> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> UserExistAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task
}
