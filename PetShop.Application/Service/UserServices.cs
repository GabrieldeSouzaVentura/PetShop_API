using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetShop.DTOs.JwtDtos;
using PetShop.Models;
using PetShop.PetShop.Application.Service.IService;

namespace PetShop.PetShop.Application.Service;

public class UserServices : IUserServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public UserServices(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        AppDbContext context,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<ApplicationUser?> FindByNameAsync(string user)
    {
        return await _userManager.FindByNameAsync(user);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password!);
    }

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
    {   
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
    {
        if (await _userManager.FindByNameAsync(user.UserName!) != null) return IdentityResult.Failed(new IdentityError { Description = "User exists" });

        if (await _userManager.FindByEmailAsync(user.Email!) != null) return IdentityResult.Failed(new IdentityError { Description = "Email exists" });

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

    public async Task<ApplicationUser> GetLoggerUserAsync()
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);

        if (user == null) throw new UnauthorizedAccessException("User not found");

        return user;
    }

    public async Task<IEnumerable<ResponseInformationsDto>> GetUserInformationsAsync()
    {
        var user = await GetLoggerUserAsync();

        var isAdmin = await IsInRoleAsync(user);

        var users = isAdmin ? await _userManager.Users.ToListAsync() : new List<ApplicationUser> { user };

        var dtoList = _mapper.Map<IEnumerable<ResponseInformationsDto>>(users);

        foreach (var dto in dtoList)
        {
            var appUser = users.First(u => u.Id == dto.UserId);
            var roles = await _userManager.GetRolesAsync(appUser);
            dto.Role = roles.FirstOrDefault();
        }

        return dtoList;
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user)
    {
        return await _userManager.IsInRoleAsync(user, "Admin");
    }

    public async Task<Expression<Func<T, bool>>?> GetFilterByUserAsync<T, TKey>(Expression<Func<T, TKey>> tutorIdSelector)
    {
        var user = await GetLoggerUserAsync();
        var isAdmin = await IsInRoleAsync(user);

        if (isAdmin) return null;

        return Expression.Lambda<Func<T, bool>>(
            Expression.Equal(tutorIdSelector.Body, Expression.Constant(user.TutorId, typeof(TKey))),tutorIdSelector.Parameters);
    }

    public async Task<Expression<Func<T, bool>>> GetFilterByNameAndUserAsync<T, TKey>(
        string name, Expression<Func<T, string>> nameSelector, Expression<Func<T, TKey>> tutorIdSelector)
    {
        var user = await GetLoggerUserAsync();
        var isAdmin = await IsInRoleAsync(user);

        var param = Expression.Parameter(typeof(T), "x");

        var nameProp = Expression.Invoke(nameSelector, param);
        var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
        var namePropToLower = Expression.Call(nameProp, toLowerMethod);
        var nameConstantToLower = Expression.Constant(name.ToLower());
        var nameEquals = Expression.Equal(namePropToLower, nameConstantToLower);

        Expression finalBody;

        if (isAdmin) { finalBody = nameEquals; }

        else
        {
            var tutorProp = Expression.Invoke(tutorIdSelector, param);
            var tutorIdConstant = Expression.Constant(user.TutorId);
            var tutorCondition = Expression.Equal(tutorProp, tutorIdConstant);
            finalBody = Expression.AndAlso(nameEquals, tutorCondition);
        }

        return Expression.Lambda<Func<T, bool>>(finalBody, param);
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

    public async Task<IdentityResult> DeleteUserAsync(int id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.TutorId == id);

        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found"});

        var result = await _userManager.DeleteAsync(user);

        return result;
    }

    public async Task<bool> ExistsByTutorIdAsync(int? id)
    {
        if (id == null) { return false; }
        return await _userManager.Users.AnyAsync(u => u.TutorId == id);
    }

    public async Task<ResponseInformationsDto> GetByNameUser(string name)
    {
        var user = await _userManager.FindByNameAsync(name);

        var dto = _mapper.Map<ResponseInformationsDto>(user);

        var role = await _userManager.GetRolesAsync(user!);
        dto.Role = role.FirstOrDefault();

        return dto;
    }
}
