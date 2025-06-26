using Microsoft.AspNetCore.Identity;
using PetShop.Models;

namespace PetShop.Service;

public class UserServices
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserServices(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
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

    public async Task AddRoleAsync(ApplicationUser user, string role)
    {
        var roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists) 
            await _roleManager.CreateAsync(new IdentityRole(role));

        await _userManager.AddToRoleAsync(user, role);
    }
}
