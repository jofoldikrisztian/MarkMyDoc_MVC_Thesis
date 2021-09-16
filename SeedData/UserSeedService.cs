using MarkMyDoctor.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var createResult = await userManager.CreateAsync(user, "P@$$W0rd");

                if (userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var result = await userManager.ConfirmEmailAsync(user, code);
                }

                var addToRoleResult = await userManager.AddToRoleAsync(user, "Administrator");


                if (!createResult.Succeeded || !addToRoleResult.Succeeded)
                {
                    throw new ApplicationException("Nem sikerült létrehozni az Administrator felhasználót");
                }
            }
        }
    }
}
