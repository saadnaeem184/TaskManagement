using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seed
{
    public static class IdentitySeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            string[] roleNames = { "Employer", "Employee", "Admin" }; // Added Admin for future use example
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (result.Succeeded)
                    {
                        logger.LogInformation("Role '{RoleName}' created successfully.", roleName);
                    }
                    else
                    {
                        logger.LogError("Error creating role '{RoleName}'. Errors: {Errors}", roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    logger.LogInformation("Role '{RoleName}' already exists.", roleName);
                }
            }
        }

        // Example for seeding a default admin user
        public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ILogger logger, IConfiguration configuration)
        {
            // Ensure Admin role exists (could be called after SeedRolesAsync)
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                logger.LogWarning("Admin role does not exist. Cannot seed admin user.");
                return;
            }

            var adminEmail = configuration["DefaultAdminUser:Email"] ?? "admin@example.com";
            var adminUsername = configuration["DefaultAdminUser:Username"] ?? "adminuser";
            var adminPassword = configuration["DefaultAdminUser:Password"] ?? "AdminP@sswOrd1!";


            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                AppUser adminUser = new AppUser
                {
                    UserName = adminUsername,
                    Email = adminEmail,
                    FirstName = "Default",
                    LastName = "Admin",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation("Default admin user '{AdminEmail}' created and assigned to Admin role.", adminEmail);
                }
                else
                {
                    logger.LogError("Error creating default admin user '{AdminEmail}'. Errors: {Errors}", adminEmail, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogInformation("Default admin user '{AdminEmail}' already exists.", adminEmail);
            }
        }
    }
}
