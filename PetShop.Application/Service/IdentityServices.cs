using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetShop.Application.Service.IService;
using PetShop.DTOs.JwtDtos;
using PetShop.Models;

namespace PetShop.Application.Service;

public class IdentityServices : IIdentityServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public IdentityServices(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> AddToRoleAsync(string email, string role)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        var roleExists = await _roleManager.RoleExistsAsync(role);

        if (!roleExists) await _roleManager.CreateAsync(new IdentityRole(role));

        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password!);
    }

    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
    {
        if (await _userManager.FindByNameAsync(user.UserName!) != null) return IdentityResult.Failed(new IdentityError { Description = "User exists" });

        if (await _userManager.FindByEmailAsync(user.Email!) != null) return IdentityResult.Failed(new IdentityError { Description = "Email exists" });

        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> DeleteUserAsync(int id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.TutorId == id);

        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        var result = await _userManager.DeleteAsync(user);

        return result;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<bool> ExistsByTutorIdAsync(int? id)
    {
        if (id == null) { return false; }
        return await _userManager.Users.AnyAsync(u => u.TutorId == id);
    }

    public async Task<ApplicationUser?> FindByNameAsync(string user)
    {
        return await _userManager.FindByNameAsync(user);
    }

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersByAuthorizationAsync(ApplicationUser user, bool isAdmin)
    {
        if (isAdmin)
        {
            return await _userManager.Users.ToListAsync();
        }
        else
        {
            return new List<ApplicationUser> { user };
        }
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user)
    {
        return await _userManager.IsInRoleAsync(user, "Admin");
    }

    public async Task UpdateAsync(ApplicationUser user)
    {
        await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> UpdateUser(int id, UpdateUserDto updateUserDto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.TutorId == id);

        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        if (!string.IsNullOrWhiteSpace(updateUserDto.UserName) || !string.IsNullOrWhiteSpace(updateUserDto.Email))
        {
            user.UserName = updateUserDto.UserName;
            user.Email = updateUserDto.Email;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, updateUserDto.NewPassword!);

        return result;
    }
}
