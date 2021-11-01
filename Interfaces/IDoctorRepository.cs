using MarkMyDoctor.Infrastructure;
using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarkMyDoctor.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<List<string>?> GetAutoCompleteSearchResultsAsync(string toSearch);
        Task<DoctorViewModel> CollectDataForDoctorFormAsync();
        Task<DoctorViewModel> CollectDataForDoctorFormAsync(int id);
        Task<Doctor> CreateDoctorAsync(DoctorViewModel doctorViewModel);
        Task UpdateDoctorAsync(int id, DoctorViewModel doctorViewModel);
        Task<PaginatedList<Doctor>> GetSearchResultAsync(string toSearch, int pageNumber);
        Task<PaginatedList<Doctor>> GetDoctorsAsync(int pageNumber);
    }
}
