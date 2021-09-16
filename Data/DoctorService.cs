using MarkMyDoctor.Infrastructure;
using MarkMyDoctor.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public class DoctorService : IDoctorService
    {
        public DoctorService(DoctorDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private DoctorDbContext DbContext { get; }

        private IQueryable<Doctor> Doctors => DbContext.Doctors;
        private IQueryable<Review> Reviews => DbContext.Reviews;
        private IQueryable<Speciality> Specialities => DbContext.Specialities;
        private IQueryable<City> Cities => DbContext.Cities;
        private IQueryable<Facility> Facilities => DbContext.Facilities;
        private IQueryable<DoctorFacility> DoctorFacilities => DbContext.DoctorFacilities;
        private IQueryable<DoctorSpeciality> DoctorSpecialities => DbContext.DoctorSpecialities;
        private IQueryable<User> Users => DbContext.Users;


        public async Task<PaginatedList<Doctor>> GetAllDoctorAsync(int pageNumber)
        {
            var query = Doctors.OrderBy(q => q.Name);

            var model = await PaginatedList<Doctor>.CreateAsync(query, pageNumber, 5);

            return model;
        }

        public async Task<PaginatedList<Doctor>> GetDoctorsByCityAsync(string toSearch, int pageNumber)
        {
           
            var query = Doctors.Where(d => d.DoctorFacilities.Any(f => f.Facility.City.Name.Equals(toSearch))).OrderBy(q => q.Name);

            var model = await PaginatedList<Doctor>.CreateAsync(query, pageNumber, 5);

            return model;
        }

        public async Task<PaginatedList<Doctor>> GetDoctorsByNameAsync(string toSearch, int pageNumber)
        {
            var query = Doctors.Where(d => d.Name.Contains(toSearch)).OrderBy(q => q.Name);

            var model = await PaginatedList<Doctor>.CreateAsync(query, pageNumber, 5);

            return model;
        }

        public async Task<PaginatedList<Doctor>> GetDoctorsBySpeciality(string toSearch, int pageNumber)
        {
            var query = Doctors.Where(d => d.DoctorSpecialities.Any(s => s.Speciality.Name.Equals(toSearch))).OrderBy(d => d.Name);

            var model = await PaginatedList<Doctor>.CreateAsync(query, pageNumber, 5);

            return model;
        }

        public List<string> GetSearchResults(string toSearch)
        {
            var result = Cities.Where(c => c.Name.Contains(toSearch)).Select(c => c.Name).ToList();

            result.AddRange(Doctors.Where(d => d.Name.Contains(toSearch)).Select(d => d.Name).ToList());

            result.AddRange(Specialities.Where(s => s.Name.Contains(toSearch)).Select(s => s.Name).ToList());

            result.Sort();

            return result;
        }

        public bool IsValidCity(string toSearch)
        {
            return Cities.Any(c => c.Name.Equals(toSearch));
        }

        public bool IsValidDoctor(string toSearch)
        {
            return Doctors.Any(d => d.Name.Equals(toSearch));
        }

        public bool IsValidSpeciality(string toSearch)
        {
            return Specialities.Any(s => s.Name.Equals(toSearch));
        }

        public bool DoctorExists(int id)
        {
            return Doctors.Any(e => e.Id == id);
        }

        public async Task<Doctor> GetDoctorByIdAsync(int? id)
        {
            return await Doctors.Include(d => d.DoctorSpecialities)
                                .ThenInclude(s => s.Speciality)
                                .Include(d => d.DoctorFacilities)
                                .ThenInclude(f => f.Facility)
                                .Include(d => d.Reviews)
                                .FirstOrDefaultAsync(d => d.Id.Equals(id));
        }

        public async Task SaveChangesAsync()
        {
           await DbContext.SaveChangesAsync();
        }

        public void AddDoctor(Doctor doctor)
        {
            DbContext.Add(doctor);
        }

        public void UpdateDoctor(Doctor doctor)
        {
            DbContext.Update(doctor);
        }

        public void Remove(Doctor doctor)
        {
            DbContext.Remove(doctor);
        }

        public async Task<ICollection<SelectListItem>> GetSpecialitiesToSelectListAsync()
        {
            return (from spec in await GetSpecialitiesAsync() select new SelectListItem { Value = spec.Id.ToString(), Text = spec.Name }).ToList();
        }
        public async Task<ICollection<Speciality>> GetSpecialitiesAsync()
        {
            return await Specialities.OrderBy(s => s.Name).ToListAsync();
        }

        public async IAsyncEnumerable<Speciality> GetSelectedSpecialitiesAsync(List<string> selectedSpecialityIds)
        {
            if (selectedSpecialityIds == null) yield break;
            foreach (var specId in selectedSpecialityIds)
            {
                if (int.TryParse(specId, out var tempSpecId))
                {
                    yield return await Specialities.SingleAsync(s => s.Id.Equals(tempSpecId));
                }
            }
        }

        public async Task<ICollection<DoctorSpeciality>> GetDoctorSpecialities(int id)
        {
            return await DoctorSpecialities.Where(s => s.DoctorId == id).ToListAsync();
        }

        public async Task CalculateDoctorOverall(int id, int actualReviewScore)
        {
            var doc = await GetDoctorByIdAsync(id);


            if (doc != null)
            {
                if (Reviews.Where(r => r.DoctorId == id).Any())
                {
                    var professionalism = Reviews.Where(review => review.Doctor.Id.Equals(id)).Average(review => review.ProfessionalismRating);
                    var humanity = Reviews.Where(review => review.Doctor.Id.Equals(id)).Average(review => review.HumanityRating);
                    var communication = Reviews.Where(review => review.Doctor.Id.Equals(id)).Average(review => review.CommunicationRating);
                    var empathy = Reviews.Where(review => review.Doctor.Id.Equals(id)).Average(review => review.EmpathyRating);
                    var flexibility = Reviews.Where(review => review.Doctor.Id.Equals(id)).Average(review => review.FlexibilityRating);

                    doc.OverallRating = Convert.ToByte(Math.Round((professionalism + humanity + flexibility + empathy + communication + actualReviewScore) / 5.0));
                }
                else
                {
                    doc.OverallRating = Convert.ToByte(Math.Round((actualReviewScore) / 5.0));
                }

               

                DbContext.Update(doc);
            }
        }

        public async Task CreateReview(Review review)
        {
            await DbContext.AddAsync(review);
        }

        public async Task<User> GetUserById(int id)
        {
            return await Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
