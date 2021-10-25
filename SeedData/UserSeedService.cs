using MarkMyDoctor.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.SeedData
{
    public class UserSeedService : IUserSeedService
    {
        private readonly UserManager<User> userManager;

        public UserSeedService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public async Task SeedUserAsync()
        {
            if (!(await userManager.GetUsersInRoleAsync("Administrator")).Any())
            {
                var user = new User
                {
                    UserName = "Administrator",
                    Email = "admin@markmydoc.com",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    NormalizedEmail = "ADMIN@MARKMYDOC.COM",
                    NormalizedUserName = "ADMINISTRATOR",
                    PhoneNumber = "+36303332233",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };

                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "secret");
                user.PasswordHash = hashed;

                var result = await userManager.CreateAsync(user);


                var addToRoleResult = await userManager.AddToRoleAsync(user, "Administrator");


                if (!result.Succeeded)
                {
                    throw new ApplicationException("Nem sikerült létrehozni az Administrator felhasználót");
                }

                if (!addToRoleResult.Succeeded)
                {
                    throw new ApplicationException("Nem sikerült a felhasználóhoz szerepkört rendelni.");
                }
            }
        }
    }
}
