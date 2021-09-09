using MarkMyDoctor.Infrastructure;
using MarkMyDoctor.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public interface IDoctorService
    {

        List<string> GetSearchResults(string toSearch);
        bool IsValidCity(string toSearch);
        Task<PaginatedList<Doctor>> GetDoctorsByCityAsync(string toSearch, int pageNumber);
        bool IsValidDoctor(string toSearch);
        Task<PaginatedList<Doctor>> GetDoctorsByNameAsync(string toSearch, int pageNumber);
        Task<PaginatedList<Doctor>> GetAllDoctorAsync(int pageNumber);
        bool IsValidSpeciality(string toSearch);
        Task<PaginatedList<Doctor>> GetDoctorsBySpeciality(string toSearch, int pageNumber);
    }
}
