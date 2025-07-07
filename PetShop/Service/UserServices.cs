using Microsoft.AspNetCore.Identity;
using PetShop.Models;
using PetShop.Service.IService;

namespace PetShop.Service;

public class UserServices : IUserServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserServices(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ApplicationUser?> FindByNameAsync(string user)
    {
        if (await _userManager.FindByNameAsync(user) != null) throw new Exception("User exists");

        return await _userManager.FindByNameAsync(user);
    }

    public async Task<bool> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        var userExists = _userManager.FindByNameAsync(user.UserName);
        var passowordExists = _userManager.CheckPasswordAsync(user ,password);

        if (userExists == null && passowordExists == null) throw new Exception("User or password invalid");

        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
    {
        if (await _userManager.FindByNameAsync(user.UserName) != null) return IdentityResult.Failed(new IdentityError { Description = "User exists" });

        if (await _userManager.FindByEmailAsync(user.Email) != null) return IdentityResult.Failed(new IdentityError { Description = "Email exists" });

        return await _userManager.CreateAsync(user, password);
    }

    public async Task UpdateAsync(ApplicationUser user)
    {
        await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> AddToRoleAsync(string email, string role)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        var roleExists = await _roleManager.RoleExistsAsync(role);

        if (!roleExists) await _roleManager.CreateAsync(new IdentityRole(role));

        return await _userManager.AddToRoleAsync(user, role);
    }
}
