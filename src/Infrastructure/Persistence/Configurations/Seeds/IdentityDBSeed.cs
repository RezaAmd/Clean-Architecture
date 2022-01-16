using Application.Services.Identity;
using Domain.Entities.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations.Seeds
{
    public static class IdentityDBSeed
    {
        public static async Task SeedDefaultUserAsync(UserService userService)
        {
            var defaultUser = new User("admin");
            var user = await userService.FindByNameAsync(defaultUser.Username);
            if (user == null)
                await userService.CreateAsync(defaultUser, "admin");
        }

        public static async Task SeedDefaultRoleAsync(RoleService roleManager)
        {
            var defaultRoles = new List<Role>()
            {
                new Role("Admin"),
                new Role("Developer")
            };
            await roleManager.CreateRangeAsync(defaultRoles);
        }

        public static async Task SeedDefaultRolesAssign(UserService userService, string username, List<string> roles)
        {
            if (username != null)
            {
                var user = await userService.FindByNameAsync(username);
                if (user != null)
                    await userService.AddToRolesAsync(user, roles);
            }
        }
    }
}