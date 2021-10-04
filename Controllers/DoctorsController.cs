using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MarkMyDoctor.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ILogger<DoctorsController> logger;
        private readonly IUnitOfWork unitOfWork;

        public DoctorsController(ILogger<DoctorsController> logger, IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {

            ViewBag.Action = "Index";

            try
            {
                var doctors = await unitOfWork.DoctorRepository.GetDoctorsAsync(pageNumber);
                return View(doctors);
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                logger.LogInformation("Hiba a művelet végrehajtása során: {0}", ex.Message);
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
                var doctor = await unitOfWork.DoctorRepository.GetByIdAsync(id.Value, d => d.Include(d => d.DoctorSpecialities)
                                                                                        .ThenInclude(s => s.Speciality)
                                                                                        .Include(d => d.DoctorFacilities)
                                                                                        .ThenInclude(f => f.Facility)
                                                                                        .Include(d => d.Reviews)
                                                                                        .ThenInclude(d => d.User));

                return View(doctor);
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                logger.LogError("Hiba a művelet végrehajtása során: {0}", ex.Message);
                return RedirectToAction("NoResult", "Home");
            }
        }

        // GET: Doctors/Create
        public async Task<IActionResult> Create()
        {

            try
            {
                var doctorViewModel = await unitOfWork.DoctorRepository.CollectDataForDoctorFormAsync();
                return View(doctorViewModel);
            }
            catch (Exception ex)
            {

                unitOfWork.Rollback();
                logger.LogError("Hiba a művelet végrehajtása során: {0}", ex.Message);
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
                    var doctor = await unitOfWork.DoctorRepository.CreateDoctorAsync(doctorViewModel);

                    unitOfWork.Commit();

                    logger.LogInformation("Az új orvos hozzáadásra került az adatbázishoz.");

                    return RedirectToAction("Details", "Doctors", new { id = doctor.Id });

                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    logger.LogInformation("Hiba a művelet végrehajtása során: {0}", ex.Message);
                    return RedirectToAction("SomethingWentWrong", "Home");
                }
            }
            return View(doctorViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NoResult", "Home");
            }

            try
            {
                var doctorViewModel = await unitOfWork.DoctorRepository.CollectDataForDoctorFormAsync(id.Value);

                ViewBag.returnUrl = Request.Headers["Referer"].ToString();

                return View(doctorViewModel);
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                logger.LogInformation("Hiba a művelet végrehajtása során: {0}", ex.Message);
                return RedirectToAction("NoResult", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorViewModel doctorViewModel)
        {

            try
            {
                var doctor = await unitOfWork.DoctorRepository.GetByIdAsync(id);

                if (ModelState.IsValid)
                {

                    await unitOfWork.DoctorRepository.UpdateDoctorAsync(id, doctorViewModel);

                    unitOfWork.Commit();

                    logger.LogInformation("A(z) {0} id-val rendelkező orvos adatlapja sikeresen frissítésre került.", id);

                    return RedirectToAction("Details", "Doctors", new { id = id });

                }

            }
            catch (Exception ex)
            {
                logger.LogError("Hiba az adatlap szerkesztése során:", ex.Message);

                unitOfWork.Rollback();

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
                var doctor = await unitOfWork.DoctorRepository.GetByIdAsync(id.Value);

                return View(doctor);
            }
            catch (Exception ex)
            {
                logger.LogError("Hiba történt az orvos törlése során. Hiba: {}", ex.Message);
                unitOfWork.Rollback();
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
                var doctor = await unitOfWork.DoctorRepository.GetByIdAsync(id);

                unitOfWork.DoctorRepository.Remove(doctor);

                unitOfWork.Commit();

                logger.LogInformation("A(z) {} id-val rendelkező orvos eltávolításra került az adatbázisból", id);

                return RedirectToAction("Index", "Home"); //Az orvos eltávolításra került oldal létrehozása

            }
            catch (Exception ex)
            {

                logger.LogError("Hiba történt az orvos törlése során. Hiba: {}", ex.Message);
                unitOfWork.Rollback();
                return RedirectToAction("SomethingWentWrong", "Home");
            }



        }
    }
}
