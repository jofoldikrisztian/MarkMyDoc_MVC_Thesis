using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MarkMyDoctor.Data;
using System.Linq.Dynamic.Core;
using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace MarkMyDoctor.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class UsersController : Controller
    {
        private readonly DoctorDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UsersController> _logger;

        public UsersController(DoctorDbContext context, IUnitOfWork unitOfWork, ILogger<UsersController> logger)
        {
            _context = context;
            this._unitOfWork = unitOfWork;
            this._logger = logger;
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

                var userData = await _unitOfWork.UserRepository.GetUsersAndRolesAsync();


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
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"{id} ID-val rendelkező felhasználó nem található!";
                return RedirectToAction("NoResult", "Home");
            }

            ViewBag.UserName = user.UserName;

            var model = new List<ManageUserRolesViewModel>();

            foreach (var role in await _unitOfWork.UserRepository.GetRolesAsync())
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    Id = role.Id,
                    RoleName = role.Name
                };

                if (await _unitOfWork.UserRepository.IsInRoleAsync(user, role.Name))
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
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if (user == null)
            {
                return View();
            }

            var roles = await _unitOfWork.UserRepository.GetRolesAsync(user);

            var result = await _unitOfWork.UserRepository.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Nem sikerült eltávolítani a szerepköröket!");
                return View(model);
            }

            result = await _unitOfWork.UserRepository.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));

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
                return RedirectToAction("NoResult", "Home");
            }

            ViewBag.userId = id;

            var user = await _unitOfWork.UserRepository.GetByIdAsync(id.Value);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"{id} ID-val rendelkező felhasználó nem található!";
                return RedirectToAction("NoResult", "Home");
            }

            ViewBag.UserName = user.UserName;

            var userViewmodel = new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(userViewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, UserViewModel user)
        {
            if (id == null)
            {
                return RedirectToAction("NoResult", "Home");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.UserRepository.UpdateUserAsync(user);

                    _unitOfWork.Commit();

                }
                catch (Exception ex)
                {
                    _logger.LogError("Hiba történt az orvos törlése során. Hiba: {}", ex.Message);
                    _unitOfWork.Dispose();
                    return RedirectToAction("SomethingWentWrong", "Home");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NoResult", "Home");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(id.Value);

            if (user == null)
            {
                return RedirectToAction("NoResult", "Home");
            }

            var userViewModel = new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            return View(userViewModel);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

            _unitOfWork.UserRepository.Remove(user);

            _unitOfWork.Commit();

            return RedirectToAction(nameof(Index));
        }

    }
}
