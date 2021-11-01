using MarkMyDoctor.Infrastructure;
using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        public async Task<List<string>?> GetAutoCompleteSearchResultsAsync(string toSearch)
        {

            if (toSearch.Contains("dr.", StringComparison.OrdinalIgnoreCase))
            {
                toSearch = Regex.Replace(toSearch, @"\A\bDr\b.?", "", RegexOptions.IgnoreCase).Trim();
            }

            if (toSearch.Length > 2)
            {
                var result = await dbContext.Cities.Where(c => c.Name.Contains(toSearch)).Select(c => c.Name).ToListAsync();

                var doctorNames = await dbContext.Doctors.Where(d => d.Name.Contains(toSearch)).Select(d => new { d.Name, d.IsStartWithDr }).ToListAsync();

                foreach (var docName in doctorNames)
                {
                    if (docName.IsStartWithDr)
                    {
                        result.Add("dr. " + docName.Name);
                    }
                    else { result.Add(docName.Name); }
                }

                result.AddRange(await dbContext.Specialities.Where(s => s.Name.Contains(toSearch)).Select(s => s.Name).ToListAsync());

                result.Sort();

                return result;
            }

            return null;

           
        }
        /// <summary>
        /// Létrehoz egy DoctorViewModel-t egy új orvos létréhezásához, összegyűjti a szükséges adatokat az adatbáziosból feltölti a multiselect listát, a specialitásokkal. 
        /// </summary>
        /// <returns></returns>
        public async Task<DoctorViewModel> CollectDataForDoctorFormAsync()
        {
            var dvm = new DoctorViewModel()
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


            if (dvm == null)
            {
                throw new ApplicationException("Hiba történt a model létrehozása során.");
            }

            return dvm;
        }

        public async Task<DoctorViewModel> CollectDataForDoctorFormAsync(int id)
        {
            try
            {
                var doctor = await GetByIdAsync(id, d => d.Include(d => d.DoctorSpecialities));

                var dvm =  new DoctorViewModel()
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

                if (dvm.Doctor.IsStartWithDr)
                {
                    dvm.Doctor.Name = "dr. " + doctor.Name;
                }

                return dvm;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateDoctorAsync(int id, DoctorViewModel doctorViewModel)
        {
            try
            {
                var doctor = await GetByIdAsync(doctorViewModel.Doctor.Id, d => d.Include(d => d.DoctorSpecialities));

                if (doctor == null)
                    throw new KeyNotFoundException("A keresett orvos nem található!");

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

                doctor.IsStartWithDr = doctorViewModel.Doctor.Name.StartsWith("dr.", StringComparison.OrdinalIgnoreCase);

                if (doctor.IsStartWithDr)
                {
                    doctor.Name = Regex.Replace((Regex.Replace(doctorViewModel.Doctor.Name, @"\A\bDr\b.?", "", RegexOptions.IgnoreCase).ToString()), @"\bDr\b.?", "dr. ", RegexOptions.IgnoreCase).Trim();
                }

              
                doctor.CanPayWithCard = doctorViewModel.Doctor.CanPayWithCard;
                doctor.Email = doctorViewModel.Doctor.Email;
                doctor.PhoneNumber = doctorViewModel.Doctor.PhoneNumber;
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

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Aszinkron módon létrehoz egy orvos entitást, egy DoctorViewModel-ből. 
        /// </summary>
        /// <param name="doctorViewModel"></param>
        /// <returns></returns>
        public async Task<Doctor> CreateDoctorAsync(DoctorViewModel doctorViewModel)
        {
            try
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

                doctor.IsStartWithDr = doctor.Name.StartsWith("dr.", StringComparison.OrdinalIgnoreCase);

                if (doctor.IsStartWithDr)
                {
                    doctor.Name = Regex.Replace((Regex.Replace(doctor.Name, @"\A\bDr\b.?", "", RegexOptions.IgnoreCase).ToString()), @"\bDr\b.?", "dr.", RegexOptions.IgnoreCase).Trim();
                }

                await AddAsync(doctor);

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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<PaginatedList<Doctor>> GetSearchResultAsync(string toSearch, int pageNumber, bool byName, bool byCity, bool bySpeciality)
        {
            try
            {

                IQueryable<Doctor> query;

              
                if (byName && byCity && bySpeciality)
                {
                    query = dbContext.Doctors.Where(d => d.DoctorSpecialities.Any(s => s.Speciality.Name.Contains(toSearch)) ||
                                                        d.Name.Contains(toSearch) ||
                                                        d.DoctorFacilities.Any(f => f.Facility.City.Name.Contains(toSearch)))
                                                        .OrderBy(d => d.Name);
                }
                else if(byName && byCity && !bySpeciality)
                {
                    query = dbContext.Doctors.Where(d => d.Name.Contains(toSearch) ||
                                                        d.DoctorFacilities.Any(f => f.Facility.City.Name.Contains(toSearch)))
                                                        .OrderBy(d => d.Name);
                }
                else if (byName && !byCity && bySpeciality)
                {
                    query = dbContext.Doctors.Where(d => d.DoctorSpecialities.Any(s => s.Speciality.Name.Contains(toSearch)) ||
                                           d.Name.Contains(toSearch))
                                           .OrderBy(d => d.Name);
                }
                else if (byName && !byCity && !bySpeciality)
                {
                    query = dbContext.Doctors.Where(d => d.Name.Contains(toSearch))
                                                               .OrderBy(d => d.Name);
                }
                else if (!byName && byCity && bySpeciality)
                {
                    query = dbContext.Doctors.Where(d => d.DoctorSpecialities.Any(s => s.Speciality.Name.Contains(toSearch)) ||
                                                        d.DoctorFacilities.Any(f => f.Facility.City.Name.Contains(toSearch)))
                                                        .OrderBy(d => d.Name);
                }
                else if (!byName && byCity && !bySpeciality)
                {
                    query = dbContext.Doctors.Where(d => d.DoctorFacilities.Any(f => f.Facility.City.Name.Contains(toSearch))).OrderBy(d => d.Name);
                }
                else if (!byName && !byCity && bySpeciality)
                {
                    query = dbContext.Doctors.Where(d => d.DoctorSpecialities.Any(s => s.Speciality.Name.Contains(toSearch))) 
                                                        .OrderBy(d => d.Name);
                }
                else
                {
                    query = dbContext.Doctors;
                }

                var model = await PaginatedList<Doctor>.CreateAsync(query, pageNumber, 5, byName, byCity, bySpeciality);



                if (model.Count() == 0)
                {
                    throw new KeyNotFoundException("Nincs a keresési kritériumnak megfelelő orvos.");
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginatedList<Doctor>> GetDoctorsAsync(int pageNumber)
        {
            try
            {
                var query = dbContext.Doctors.OrderBy(d => d.Name).AsQueryable();

                var model = await PaginatedList<Doctor>.CreateAsync(query, pageNumber, 5);

                if (model.Count() == 0)
                {
                    throw new KeyNotFoundException("Nincs a keresési kritériumnak megfelelő orvos.");
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
