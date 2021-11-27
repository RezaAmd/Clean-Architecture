using Application.Services.Identity;
using Domain.Entities.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations.Seeds
{
    public static class IdentityDBSeed
    {
        public static async Task SeedDefaultUserAsync(UserService userManager)
        {
            var defaultUser = new User("sample");
            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
                await userManager.CreateAsync(defaultUser, "123456");
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