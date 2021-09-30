using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        /// <returns>A keresési értéket tartalmazó stringek</returns>
        public List<string> GetAutoCompleteSearchResults(string toSearch)
        {
            var result = dbContext.Cities.Where(c => c.Name.Contains(toSearch)).Select(c => c.Name).ToList();

            result.AddRange(dbContext.Doctors.Where(d => d.Name.Contains(toSearch)).Select(d => d.Name).ToList());

            result.AddRange(dbContext.Specialities.Where(s => s.Name.Contains(toSearch)).Select(s => s.Name).ToList());

            result.Sort();

            return result;
        }

        public async Task<DoctorViewModel> CollectDataForANewDoctorAsync()
        {
            return new DoctorViewModel()
            {
                Specialities = (from spec in await dbContext.Specialities
                                                            .OrderBy(s => s.Name)
                                                            .ToListAsync()
                                select new SelectListItem
                                {
                                    Value = spec.Id.ToString(),
                                    Text = spec.Name }
                                ).ToList()
            }; 
        }
    }
}
