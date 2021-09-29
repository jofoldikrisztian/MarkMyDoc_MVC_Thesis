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

namespace MarkMyDoctor.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly IDoctorService DoctorService;


        public DoctorsController(IDoctorService doctorService)
        {
            DoctorService = doctorService;
          
        }


        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await DoctorService.GetDoctorByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public async Task<IActionResult> Create()
        {
            var doctorViewModel = new DoctorViewModel()
            {
                Specialities = await DoctorService.GetSpecialitiesToSelectListAsync(),
            };

            return View(doctorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorViewModel doctorViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var selectedSpecialities = new List<Speciality>();
                    var doctor = new Doctor();

                    await foreach (var spec in DoctorService.GetSelectedSpecialitiesAsync(doctorViewModel.SelectedSpecialityIds))
                    {
                        selectedSpecialities.Add(spec);
                    }

                    if (doctorViewModel.Image != null)
                    {
                        if (doctorViewModel.Image.Length > 0)
                        {   

                            byte[]? p1 = null;
                            using (var target = new MemoryStream())
                            {
                                await doctorViewModel.Image.CopyToAsync(target);
                                p1 = target.ToArray();
                            }
                            doctor.PorfilePicture = p1;
                        }
                    }

                    doctor = doctorViewModel.Doctor;

                    await DoctorService.CreateDoctor(doctor);

                    var docSpecList = new List<DoctorSpeciality>();

                    foreach (var item in selectedSpecialities)
                    {
                        var docSpec = new DoctorSpeciality()
                        {
                            Speciality = item,
                            Doctor = doctor
                        };

                        docSpecList.Add(docSpec);
                    }

                    await DoctorService.AddDoctorSpecialities(docSpecList);

                    await DoctorService.SaveChangesAsync();

                    return RedirectToAction("Details", "Doctors", new { id = doctor.Id });

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

            var doctor = await DoctorService.GetDoctorByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            var doctorViewModel = new DoctorViewModel()
            {
                Specialities = await DoctorService.GetSpecialitiesToSelectListAsync(),
                Doctor = doctor,
                SelectedSpecialityIds = doctor.DoctorSpecialities.Select(d => d.Speciality.Id.ToString()).ToList()
            };

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
            var doctor = await DoctorService.GetDoctorByIdAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            var selectedSpecialities = new List<Speciality>();


            if (ModelState.IsValid)
            {
                try
                {

                    await foreach (var spec in DoctorService.GetSelectedSpecialitiesAsync(doctorViewModel.SelectedSpecialityIds))
                    {
                        selectedSpecialities.Add(spec);
                    }

                    if (doctorViewModel.Image != null)
                    {
                        if (doctorViewModel.Image.Length > 0)
                        {
                            byte[]? p1 = null;
                            using (var target = new MemoryStream())
                            {
                                await doctorViewModel.Image.CopyToAsync(target);
                                                       
                                p1 = target.ToArray();
                            }

                            doctor.PorfilePicture = p1;
                        }
                    }

                    doctor.Name = doctorViewModel.Doctor.Name;
                    doctor.CanPayWithCard = doctorViewModel.Doctor.CanPayWithCard;
                    doctor.Email = doctorViewModel.Doctor.Email;
                    doctor.PhoneNumber = doctorViewModel.Doctor.PhoneNumber;
                    //doctor.PorfilePicture = doctorViewModel.Doctor.PorfilePicture;
                    doctor.WebAddress = doctorViewModel.Doctor.WebAddress;

                    //A megszüntetett kijelölésű specialitások eltávolítása az orvos specialitásai közül
                    doctor.DoctorSpecialities.Where(m => !selectedSpecialities.Contains(m.Speciality)).ToList().ForEach(spec => doctor.DoctorSpecialities.Remove(spec));

                    //A "megmaradt" specialitások tárolása egy ideiglenes objektumba
                    var existingSpecialities = doctor.DoctorSpecialities.Select(m => m.Speciality.Id).ToList();

                    //Az új specialitások hozzáadása az orvoshoz

                    foreach (var speciality in selectedSpecialities)
                    {
                        if (!existingSpecialities.Any(ex => ex.Equals(speciality.Id)))
                        {
                            doctor.DoctorSpecialities.Add(new DoctorSpeciality() { 
                                SpecialityId = speciality.Id,
                                DoctorId = id
                            });
                        }
                    }

                    await DoctorService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorService.DoctorExists(doctor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Doctors", new { id = id });
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

            var doctor = await DoctorService.GetDoctorByIdAsync(id);

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
            DoctorService.Remove(doctor);
            await DoctorService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
