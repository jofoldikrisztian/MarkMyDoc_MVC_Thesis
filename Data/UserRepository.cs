using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly DoctorDbContext _context;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public UserRepository(UserManager<User> userManager, DoctorDbContext context, RoleManager<IdentityRole<int>> roleManager)
            :base(context)
        {
            this._userManager = userManager;
            this._context = context;
            this._roleManager = roleManager;
        }

        public async Task<IEnumerable<UserRolesViewModel>> GetUsersAndRolesAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (User user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.Id = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.UserName = user.UserName;
                thisViewModel.PhoneNumber = user.PhoneNumber;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }

            return userRolesViewModel;
        }

        public async Task<IEnumerable<IdentityRole<int>>> GetRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        private async Task<string> GetUserRoles(User user)
        {
            var roles = new List<string>(await _userManager.GetRolesAsync(user));

            return roles.Aggregate((x, y) => x + ", " + y);
        }

        public async Task<bool> IsInRoleAsync(User user, string name)
        {
            return await _userManager.IsInRoleAsync(user, name);
        }

        public async Task<IEnumerable<string>> GetRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles)
        {
            return await _userManager.RemoveFromRolesAsync(user, roles);
        }

        public async Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles)
        {
            return await _userManager.AddToRolesAsync(user, roles);
        }
    }
}
