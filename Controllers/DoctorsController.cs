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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber,CanPayWithCard,Email,WebAddress,PorfilePicture,OverallRating")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                DoctorService.AddDoctor(doctor);
                await DoctorService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
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


            if (doctorViewModel == null)
            {
                return NotFound();
            }

            return View(doctorViewModel);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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


                    doctor.Name = doctorViewModel.Doctor.Name;
                    doctor.CanPayWithCard = doctorViewModel.Doctor.CanPayWithCard;
                    doctor.Email = doctorViewModel.Doctor.Email;
                    doctor.PhoneNumber = doctorViewModel.Doctor.PhoneNumber;
                    doctor.PorfilePicture = doctorViewModel.Doctor.PorfilePicture;
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

        // GET: Doctors/Delete/5
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
