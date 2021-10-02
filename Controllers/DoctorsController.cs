using MarkMyDoctor.Data;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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

            var doctors = await unitOfWork.DoctorRepository.GetDoctorsAsync(pageNumber);

            if (doctors.Count() > 0)
            {
                return View(doctors);
            }
            else
            {
                return RedirectToAction("NoResult", "Home");
            }
        }


        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await unitOfWork.DoctorRepository.GetByIdAsync(id.Value, d => d.Include(d => d.DoctorSpecialities)
                                                                                        .ThenInclude(s => s.Speciality)
                                                                                        .Include(d => d.DoctorFacilities)
                                                                                        .ThenInclude(f => f.Facility)
                                                                                        .Include(d => d.Reviews)
                                                                                        .ThenInclude(d => d.User));
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public async Task<IActionResult> Create()
        {
            var doctorViewModel = await unitOfWork.DoctorRepository.CollectDataForDoctorFormAsync();

            if (doctorViewModel != null)
            {
                return View(doctorViewModel);
            }

            return NotFound();
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

                    unitOfWork.Save();

                    if (doctor != null)
                    {
                        return RedirectToAction("Details", "Doctors", new { id = doctor.Id });
                    }

                    return NotFound();

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View(doctorViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorViewModel = await unitOfWork.DoctorRepository.CollectDataForDoctorFormAsync(id.Value);

            ViewBag.returnUrl = Request.Headers["Referer"].ToString();

            if (doctorViewModel == null)
            {
                return NotFound();
            }

            return View(doctorViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorViewModel doctorViewModel)
        {
            var doctor = await unitOfWork.DoctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                if (await unitOfWork.DoctorRepository.UpdateDoctorAsync(id, doctorViewModel))
                {
                    return RedirectToAction("Details", "Doctors", new { id = id });
                }
                else
                {
                    return NotFound();
                }

            }

            return View(doctorViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await unitOfWork.DoctorRepository.GetByIdAsync(id.Value);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await unitOfWork.DoctorRepository.GetByIdAsync(id);

            unitOfWork.DoctorRepository.Remove(doctor);

            unitOfWork.Save();

            return RedirectToAction("SearchResult", "Search");
        }
    }
}
