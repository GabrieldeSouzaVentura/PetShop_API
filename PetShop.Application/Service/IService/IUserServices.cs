using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using PetShop.DTOs.JwtDtos;
using PetShop.Models;

namespace PetShop.PetShop.Application.Service.IService;

public interface IUserServices
{
    Task<ApplicationUser?> FindByNameAsync(string user);//
    Task<bool> ExistsByTutorIdAsync(int? id);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);//
    Task<IList<string>> GetRolesAsync(ApplicationUser user);//
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);//
    Task UpdateAsync(ApplicationUser user);//
    Task<IdentityResult> AddToRoleAsync(string email, string role);//
    Task<ApplicationUser> GetLoggerUserAsync();//
    Task<bool> IsInRoleAsync(ApplicationUser user);//
    Task<IEnumerable<ResponseInformationsDto>> GetUserInformationsAsync();//
    Task<Expression<Func<T, bool>>?> GetFilterByUserAsync<T, TKey>(Expression<Func<T, TKey>> tutorIdSelector);
    Task<Expression<Func<T, bool>>> GetFilterByNameAndUserAsync<T, TKey>(
        string name, Expression<Func<T, string>> nameSelector, Expression<Func<T, TKey>> tutorIdSelector);
    Task<IdentityResult> UpdateUser(int id, UpdateUserDto updateUserDto);//
    Task<IdentityResult> DeleteUserAsync(int id);//
    Task<ResponseInformationsDto> GetByNameUser(string name);//
}
