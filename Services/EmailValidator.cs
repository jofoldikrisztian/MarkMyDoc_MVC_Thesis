//using MarkMyDoctor.Models.Entities;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace MarkMyDoctor.Services
//{
//    public class EmailValidator<T> : IUserValidator<User> where T : User
//    {
//        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
//        {

//                if (user.Email == null)
//                {
//                    return Task.FromResult(IdentityResult.Success);
//                }
//                else
//                {

//                    var userExist = manager.FindByEmailAsync(user.Email);
//                    if (userExist.Result == null)
//                    {
//                        return Task.FromResult(IdentityResult.Success);
//                    }

//                if (userExist.Result != null && manager.IsInRoleAsync())
//                {

//                }

//                }

//                return Task.FromResult(
//                         IdentityResult.Failed(new IdentityError
//                         {
//                             Code = "400",
//                             Description = "Az email cím nem egyedi."
//                         }));
       
//        }
//    }
//}
