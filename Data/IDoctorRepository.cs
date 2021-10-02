using MarkMyDoctor.Infrastructure;
using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        List<string> GetAutoCompleteSearchResults(string toSearch);
        Task<DoctorViewModel> CollectDataForDoctorFormAsync();
        Task<DoctorViewModel> CollectDataForDoctorFormAsync(int id);
        Task<Doctor> CreateDoctorAsync(DoctorViewModel doctorViewModel);
        Task<bool> UpdateDoctorAsync(int id, DoctorViewModel doctorViewModel);
        Task<PaginatedList<Doctor>> GetSearchResultAsync(string toSearch, int pageNumber);
        Task<PaginatedList<Doctor>> GetDoctorsAsync(int pageNumber);
    }
}
