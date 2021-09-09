using MarkMyDoctor.Infrastructure;
using MarkMyDoctor.Models.Entities;
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
            return await Doctors.Include(d => d.DoctorSpecialities).ThenInclude(s => s.Speciality).Include(d => d.DoctorFacilities).ThenInclude(f => f.Facility).FirstOrDefaultAsync(d => d.Id.Equals(id));
        }

        public Task SaveChangesAsync()
        {
           return DbContext.SaveChangesAsync();
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
    }
}
