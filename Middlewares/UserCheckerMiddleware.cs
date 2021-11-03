using MarkMyDoctor.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Middlewares
{
    public class UserCheckerMiddleware
    {
        private readonly RequestDelegate _next;

        public UserCheckerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            if (!string.IsNullOrEmpty(httpContext.User.Identity.Name))
            {
                var user = await userManager.FindByNameAsync(httpContext.User.Identity.Name);

                if (user.LockoutEnd > DateTimeOffset.Now)
                {
                    //A felhasználó kijelentkeztetése, és visszairányítása a főoldalra
                    await signInManager.SignOutAsync();
                    httpContext.Response.Redirect("/");
                }
            }
            await _next(httpContext);
        }
    }
}
