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
        Task<DoctorViewModel> CollectDataForANewDoctorAsync();
    }
}
