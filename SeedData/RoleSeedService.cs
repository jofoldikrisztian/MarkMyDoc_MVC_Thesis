using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MarkMyDoctor.SeedData
{
    public class RoleSeedService : IRoleSeedService
    {
        private readonly RoleManager<IdentityRole<int>> roleManager;

        public RoleSeedService(RoleManager<IdentityRole<int>> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task SeedRoleAsync()
        {
            if (!await roleManager.RoleExistsAsync("Administrator"))
                await roleManager.CreateAsync(new IdentityRole<int> { Name = "Administrator" });

            if (!await roleManager.RoleExistsAsync("Moderator"))
                await roleManager.CreateAsync(new IdentityRole<int> { Name = "Moderator" });

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole<int> { Name = "User" });

        }
    }
}
