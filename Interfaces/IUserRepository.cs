using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {

        Task<IEnumerable<UserRolesViewModel>> GetUsersAndRolesAsync();
        Task<IEnumerable<string>> GetRolesAsync(User user);
        Task<IEnumerable<IdentityRole<int>>> GetRolesAsync();
        Task<bool> IsInRoleAsync(User user, string name);
        Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles);
        Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles);
        Task UpdateUserAsync(UserViewModel user);
    }
}
