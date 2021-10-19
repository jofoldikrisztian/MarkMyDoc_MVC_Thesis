using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarkMyDoctor.Data;
using MarkMyDoctor.Models.Entities;
using System.Linq.Dynamic.Core;
using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models.ViewModels;

namespace MarkMyDoctor.Controllers
{
    public class UsersController : Controller
    {
        private readonly DoctorDbContext _context;
        private readonly IUnitOfWork unitOfWork;

        public UsersController(DoctorDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            this.unitOfWork = unitOfWork;
        }

        // GET: Users
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadUsers()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();


                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();


                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                var userData = await unitOfWork.UserRepository.GetUsersAndRolesAsync();


                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = userData.Where(m => m.UserName.Contains(searchValue));
                }

                //total number of rows counts     
                recordsTotal = userData.Count();
                //Paging   
                var data = userData.Skip(skip).Take(20).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int id)
        {
            ViewBag.userId = id;
            var user = await unitOfWork.UserRepository.GetByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"{id} ID-val rendelkező felhasználó nem található!";
                return RedirectToAction("NoResult", "Home");
            }

            ViewBag.UserName = user.UserName;

            var model = new List<ManageUserRolesViewModel>();

            foreach (var role in await unitOfWork.UserRepository.GetRolesAsync())
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    Id = role.Id,
                    RoleName = role.Name
                };

                if (await unitOfWork.UserRepository.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }

                model.Add(userRolesViewModel);

            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, int id)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(id);

            if (user == null)
            {
                return View();
            }

            var roles = await unitOfWork.UserRepository.GetRolesAsync(user);

            var result = await unitOfWork.UserRepository.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Nem sikerült eltávolítani a szerepköröket!");
                return View(model);
            }

            result = await unitOfWork.UserRepository.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Nem sikerült hozzáadni a szerepköröket a felhasználóhoz!");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
