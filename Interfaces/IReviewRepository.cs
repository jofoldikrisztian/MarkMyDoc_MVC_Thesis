using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarkMyDoctor.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {

        Task CreateAsync(DoctorReviewViewModel doctorReviewViewModel);
        Task<IEnumerable<UnApprovedReviewViewModel>> GetUnApprovedReviewsAsync();
        Task ApproveReviewAsync(int id);

    }
}
