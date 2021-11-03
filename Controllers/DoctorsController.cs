using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DoctorsController(ILogger<DoctorsController> logger, IUnitOfWork unitOfWork)
        {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(char character, int pageNumber = 1)
        {

            ViewBag.Action = "Index";

            try
            {
                var doctors = await _unitOfWork.DoctorRepository.GetDoctorsAsync(pageNumber, character);

              return View(doctors);
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                _logger.LogInformation("Hiba a művelet végrehajtása során: {0}", ex.Message);
                return RedirectToAction("NoResult", "Home");
            }
        }


        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NoResult", "Home");
            }

            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id.Value, d => d.Include(d => d.DoctorSpecialities)
                                                                                        .ThenInclude(s => s.Speciality)
                                                                                        .Include(d => d.DoctorFacilities)
                                                                                        .ThenInclude(f => f.Facility)
                                                                                        .Include(d => d.Reviews)
                                                                                        .ThenInclude(d => d.User));

                return View(doctor);
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                _logger.LogError("Hiba a művelet végrehajtása során: {0}", ex.Message);
                return RedirectToAction("NoResult", "Home");
            }
        }

        // GET: Doctors/Create
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> Create()
        {

            try
            {
                var doctorViewModel = await _unitOfWork.DoctorRepository.CollectDataForDoctorFormAsync();
                return View(doctorViewModel);
            }
            catch (Exception ex)
            {

                _unitOfWork.Dispose();
                _logger.LogError("Hiba a művelet végrehajtása során: {0}", ex.Message);
                return RedirectToAction("SomethingWentWrong", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorViewModel doctorViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var doctor = await _unitOfWork.DoctorRepository.CreateDoctorAsync(doctorViewModel);

                    _unitOfWork.Commit();

                    _logger.LogInformation("Az új orvos hozzáadásra került az adatbázishoz.");

                    return RedirectToAction("Details", "Doctors", new { id = doctor.Id });

                }
                catch (Exception ex)
                {
                    _unitOfWork.Dispose();
                    _logger.LogInformation("Hiba a művelet végrehajtása során: {0}", ex.Message);
                    return RedirectToAction("SomethingWentWrong", "Home");
                }
            }
            return View(doctorViewModel);
        }

        [HttpGet]
        [Authorize(Roles ="Administrator,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("NoResult", "Home");

            try
            {
                var doctorViewModel = await _unitOfWork.DoctorRepository.CollectDataForDoctorFormAsync(id.Value);
                ViewBag.returnUrl = Request.Headers["Referer"].ToString();
                return View(doctorViewModel);
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                _logger.LogInformation("Hiba a művelet végrehajtása során: {0}", ex.Message);
                return RedirectToAction("NoResult", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorViewModel doctorViewModel)
        {

            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id);

                if (ModelState.IsValid)
                {

                    await _unitOfWork.DoctorRepository.UpdateDoctorAsync(id, doctorViewModel);

                    _unitOfWork.Commit();

                    _logger.LogInformation("A(z) {0} id-val rendelkező orvos adatlapja sikeresen frissítésre került.", id);

                    return RedirectToAction("Details", "Doctors", new { id = id });

                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Hiba az adatlap szerkesztése során:", ex.Message);

                _unitOfWork.Dispose();

                return RedirectToAction("SomethingWentWrong", "Home");
            }

            return View(doctorViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {


            if (id == null)
            {
                return RedirectToAction("NoResult", "Home");
            }

            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id.Value);

                var deleteDoctorViewModel = new DeleteDoctorViewModel()
                {
                    Id = doctor.Id,
                    Email = doctor.Email,
                    Name =  doctor.IsStartWithDr == true ? $"dr. {doctor.Name}" : doctor.Name, 
                    PhoneNumber = doctor.PhoneNumber
                };

                return View(deleteDoctorViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Hiba történt az orvos törlése során. Hiba: {}", ex.Message);
                _unitOfWork.Dispose();
                return RedirectToAction("SomethingWentWrong", "Home");
            }

        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id);

                _unitOfWork.DoctorRepository.Remove(doctor);

                _unitOfWork.Commit();

                _logger.LogInformation("A(z) {} id-val rendelkező orvos eltávolításra került az adatbázisból", id);

                return RedirectToAction("Index", "Home"); //TODO Az orvos eltávolításra került oldal létrehozása

            }
            catch (Exception ex)
            {

                _logger.LogError("Hiba történt az orvos törlése során. Hiba: {}", ex.Message);
                _unitOfWork.Dispose();
                return RedirectToAction("SomethingWentWrong", "Home");
            }

        }

        // GET: Doctors
        [Authorize(Roles = "Administrator")]
        public IActionResult Manage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadDoctors()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Indulási érték (hányadiktól) 
                var start = Request.Form["start"].FirstOrDefault();

                // Keresési érték (search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();


                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                var doctorData = await _unitOfWork.DoctorRepository.GetDoctorsToManageAsync();


                if (!string.IsNullOrEmpty(searchValue))
                {
                    doctorData = doctorData.Where(m => m.Name.Contains(searchValue));
                }

                //rekordok száma  
                recordsTotal = doctorData.Count();
                //oldalazása   
                var data = doctorData.Skip(skip).Take(20).ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                _logger.LogError("Hiba történt az értékelések betöltése során. Hiba: {}", ex.Message);
                return RedirectToAction("SomethingWentWrong", "Home");
            }
        }
    }
}
