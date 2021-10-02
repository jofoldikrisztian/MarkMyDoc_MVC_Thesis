using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public interface IReviewRepository : IRepository<Review>
    {

        Task<bool> Create(DoctorReviewViewModel doctorReviewViewModel, User user);

    }
}
