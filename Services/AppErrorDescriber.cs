using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Services
{
    public class AppErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            var error = base.DuplicateEmail(email);
            error.Description = "Ez az email cím már használatban van!";
            return error;
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            var error = base.DuplicateUserName(userName);
            error.Description = "Ez a felhasználónév már használatban van!";
            return error;
        }
    }
}
