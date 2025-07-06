using Microsoft.AspNetCore.Identity;
using PetShop.Models;

namespace PetShop.Service.IService;

public interface IUserServices
{
    Task<ApplicationUser?> FindByNameAsync(string user);
    Task<bool> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
    Task UpdateAsync(ApplicationUser user);
    Task<IdentityResult> AddToRoleAsync(string email, string role);
}
