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
                var thisViewModel = new UserRolesViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = await GetUserRoles(user),
                    IsLockedOut = await _userManager.IsLockedOutAsync(user) ? "Igen" : "Nem"
                };
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

        public async Task UpdateUserAsync(UserViewModel user)
        {
            var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(user.Id));

            if (userToUpdate == null)
                throw new KeyNotFoundException("A keresett felhasználó nem található!");

            userToUpdate.UserName = user.UserName;
            userToUpdate.PhoneNumber = user.PhoneNumber;
            userToUpdate.Email = user.Email;
            userToUpdate.NormalizedUserName = user.UserName.ToUpper();
            userToUpdate.NormalizedEmail = user.Email.ToUpper();

            if (user.Password != null)
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(userToUpdate, user.Password);
                userToUpdate.PasswordHash = hashed;
            }


        }

        public void LockoutUser(User user)
        {
                _userManager.SetLockoutEnabledAsync(user, true);

                DateTime lockoutDate = DateTime.Now.AddYears(100);

                _userManager.SetLockoutEndDateAsync(user, lockoutDate);

        }
    }
}
