using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TopCal.Data.Entities;

namespace TopCal.Data.Identity
{
    public class ApplicationIdentityInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationIdentityInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            await SeedRole("Admin");
            await SeedRole("Manager");
            await SeedRole("Regular");

            // Add Users
            await SeedUser("admin", "Admin");
            await SeedUser("managerUser", "Manager");
            await SeedUser("regularUser", "Regular");

        }

        private async Task SeedRole(string roleName)
        {
            if (!(await _roleManager.RoleExistsAsync(roleName)))
            {
                var role = new IdentityRole(roleName);
                await _roleManager.CreateAsync(role);
            }
        }

        private async Task SeedUser(string userName, string role)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = $"{userName}@yopmail.com"
                };

                var userResult = await _userManager.CreateAsync(user, "P@ssw0rd!");
                var roleResult = await _userManager.AddToRoleAsync(user, role);
                var claimResult = await _userManager.AddClaimAsync(user, new Claim(role, "True"));

                if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }
        }
    }
}
