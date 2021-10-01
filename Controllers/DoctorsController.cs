using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarkMyDoctor.Data;
using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Logging;

namespace MarkMyDoctor.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorService DoctorService;
        private readonly ILogger<DoctorsController> logger;
        private readonly IUnitOfWork unitOfWork;

        public DoctorsController(IDoctorService doctorService, ILogger<DoctorsController> logger, IUnitOfWork unitOfWork)
        {
            DoctorService = doctorService;
            this.logger = logger;
            this.unitOfWork = unitOfWork;
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

                if (await unitOfWork.DoctorRepository.UpdateDoctor(id, doctorViewModel))
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
            var doctor = await DoctorService.GetDoctorByIdAsync(id);
            
            unitOfWork.DoctorRepository.Remove(doctor);

            unitOfWork.Save();

            return RedirectToAction("SearchResult", "Search");
        }
    }
}
