using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using System.Threading.Tasks;

namespace MarkMyDoctor.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {

        Task<bool> CreateAsync(DoctorReviewViewModel doctorReviewViewModel, User user);

    }
}
