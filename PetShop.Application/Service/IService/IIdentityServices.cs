using Microsoft.AspNetCore.Identity;
using PetShop.DTOs.JwtDtos;
using PetShop.Models;

namespace PetShop.Application.Service.IService;

public interface IIdentityServices
{
    Task<ApplicationUser?> FindByNameAsync(string user);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
    Task UpdateAsync(ApplicationUser user);
    Task<IdentityResult> AddToRoleAsync(string email, string role);
    Task<bool> IsInRoleAsync(ApplicationUser user);
    Task<IdentityResult> UpdateUser(int id, UpdateUserDto updateUserDto);
    Task<IdentityResult> DeleteUserAsync(int id);
    Task<bool> ExistsByTutorIdAsync(int? id);
    Task<IEnumerable<ApplicationUser>> GetUsersByAuthorizationAsync(ApplicationUser applicationUser, bool isAdmin);
}
