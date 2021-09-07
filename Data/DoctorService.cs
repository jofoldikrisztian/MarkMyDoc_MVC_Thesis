using MarkMyDoctor.Models.Entities;
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


        public List<string> GetSearchResults(string toSearch)
        {
            var result = Cities.Where(c => c.Name.Contains(toSearch)).Select(c => c.Name).ToList();

            result.AddRange(Doctors.Where(d => d.Name.Contains(toSearch)).Select(d => d.Name).ToList());

            result.Sort();

            return result;
        }

    }
}
