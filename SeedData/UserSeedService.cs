﻿using MarkMyDoctor.Data;
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
        private readonly IDoctorService doctorService;

        public UserSeedService(UserManager<User> userManager, IDoctorService doctorService)
        {
            this.userManager = userManager;
            this.doctorService = doctorService;
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


                if (!result.Succeeded || !addToRoleResult.Succeeded)
                {
                    throw new ApplicationException("Nem sikerült létrehozni az Administrator felhasználót");
                }
            }
        }
    }
}
