using Microsoft.AspNetCore.Identity;
using PetShop.Models;

namespace PetShop.Context;

public static class DbInitializer
{
    public static async Task SeedRolesAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var roles = new[] { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role)) await roleManager.CreateAsync(new IdentityRole(role));
        }

        var userEmail = "user@gmail.com";
        var userPassword = "User@123";

        var userExists = await userManager.FindByEmailAsync(userEmail);
        if (userExists == null)
        {
            var userUser = new ApplicationUser
            {
                UserName = "user",
                Email = userEmail,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(userUser, userPassword);

            if (!result.Succeeded){ throw new Exception("Create user invalid"); }

            var isInRole = await userManager.IsInRoleAsync(userUser, "User");

            if (!isInRole) await userManager.AddToRoleAsync(userUser, "User");
        }

        var adminEmail = "admin@gmail.com";
        var adminPassword = "Admin@123";

        var adminExists = await userManager.FindByEmailAsync(adminEmail);
        if (adminExists == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (!result.Succeeded) { throw new Exception("Create user invalid"); }

            var isInRole = await userManager.IsInRoleAsync(adminUser, "Admin");

            if (!isInRole) await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
