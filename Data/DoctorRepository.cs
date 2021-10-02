using MarkMyDoctor.Infrastructure;
using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private readonly DoctorDbContext dbContext;

        public DoctorRepository(DoctorDbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Az autocomplete funkciót kiszolgáló metódus. Feladata, hogy az adatbázisból visszaadja a keresési paramétert tartalmazó orvosokat, városokat, specialitásokat.
        /// </summary>
        /// <param name="toSearch"></param>
        /// <returns>A keresési értéket tartalmazó stringeket</returns>
        public List<string> GetAutoCompleteSearchResults(string toSearch)
        {
            var result = dbContext.Cities.Where(c => c.Name.Contains(toSearch)).Select(c => c.Name).ToList();

            result.AddRange(dbContext.Doctors.Where(d => d.Name.Contains(toSearch)).Select(d => d.Name).ToList());

            result.AddRange(dbContext.Specialities.Where(s => s.Name.Contains(toSearch)).Select(s => s.Name).ToList());

            result.Sort();

            return result;
        }
        /// <summary>
        /// Létrehoz egy DoctorViewModel-t egy új orvos létréhezásához, összegyűjti a szükséges adatokat az adatbáziosból feltölti a multiselect listát, a specialitásokkal. 
        /// </summary>
        /// <returns></returns>
        public async Task<DoctorViewModel> CollectDataForDoctorFormAsync()
        {
            return new DoctorViewModel()
            {
                Specialities = (from spec in await dbContext.Specialities
                                                            .OrderBy(s => s.Name)
                                                            .ToListAsync()
                                select new SelectListItem
                                {
                                    Value = spec.Id.ToString(),
                                    Text = spec.Name
                                }).ToList()
            }; 
        }

        public async Task<DoctorViewModel> CollectDataForDoctorFormAsync(int id)
        {
            var doctor = await GetByIdAsync(id, d => d.Include(d => d.DoctorSpecialities));

            return new DoctorViewModel()
            {
                Specialities = (from spec in await dbContext.Specialities
                                                            .OrderBy(s => s.Name)
                                                            .ToListAsync()
                                select new SelectListItem
                                {
                                    Value = spec.Id.ToString(),
                                    Text = spec.Name
                                }).ToList(),
                Doctor = doctor,
                SelectedSpecialityIds = doctor.DoctorSpecialities.Select(d => d.Speciality.Id.ToString()).ToList()
            };
        }

        public async Task<bool> UpdateDoctorAsync(int id, DoctorViewModel doctorViewModel)
        {
            try
            {
                var doctor = await GetByIdAsync(doctorViewModel.Doctor.Id, d => d.Include(d => d.DoctorSpecialities));

                if (doctor == null)
                    return false;

                var selectedSpecialities = new List<Speciality>();

                await foreach (var spec in GetSelectedSpecialitiesAsync(doctorViewModel.SelectedSpecialityIds))
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

                        doctor.ProfilePicture = p1;
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
                        doctor.DoctorSpecialities.Add(new DoctorSpeciality()
                        {
                            SpecialityId = speciality.Id,
                            DoctorId = doctorViewModel.Doctor.Id
                        });
                    }
                }

                await dbContext.SaveChangesAsync();

                return true;
             
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        /// <summary>
        /// Aszinkron módon létrehoz egy orvos entitást, egy DoctorViewModel-ből. 
        /// </summary>
        /// <param name="doctorViewModel"></param>
        /// <returns></returns>
        public async Task<Doctor> CreateDoctorAsync(DoctorViewModel doctorViewModel)
        {
            var selectedSpecialities = new List<Speciality>();
            var doctor = new Doctor();

            await foreach (var spec in GetSelectedSpecialitiesAsync(doctorViewModel.SelectedSpecialityIds))
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
                    doctor.ProfilePicture = p1;
                }
            }

            doctor = doctorViewModel.Doctor;

            await dbContext.Doctors.AddAsync(doctor);

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

            await dbContext.DoctorSpecialities.AddRangeAsync(docSpecList);

            return doctor;

        }

        public async Task<PaginatedList<Doctor>> GetSearchResultAsync(string toSearch, int pageNumber)
        {
            var query = dbContext.Doctors.Where(
                d =>
                d.DoctorSpecialities.Any(s => s.Speciality.Name.Contains(toSearch)) ||
                d.Name.Contains(toSearch) ||
                d.DoctorFacilities.Any(f => f.Facility.City.Name.Contains(toSearch))
                ).OrderBy(d => d.Name);

            var model = await PaginatedList<Doctor>.CreateAsync(query, pageNumber, 5);

            return model;
        }

        public async Task<PaginatedList<Doctor>> GetDoctorsAsync(int pageNumber)
        {
            var query = dbContext.Doctors.OrderBy(d => d.Name).AsQueryable();

            var model = await PaginatedList<Doctor>.CreateAsync(query, pageNumber, 5);

            return model;

        }



        private async IAsyncEnumerable<Speciality> GetSelectedSpecialitiesAsync(List<string> selectedSpecialityIds)
        {
            if (selectedSpecialityIds == null) yield break;
            foreach (var specId in selectedSpecialityIds)
            {
                if (int.TryParse(specId, out var tempSpecId))
                {
                    yield return await dbContext.Specialities.SingleAsync(s => s.Id.Equals(tempSpecId));
                }
            }
        }


    }
}
