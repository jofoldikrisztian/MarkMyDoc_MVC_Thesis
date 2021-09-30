using MarkMyDoctor.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(DoctorDbContext dbContext)
            : base(dbContext)
        {

        }

    }
}
