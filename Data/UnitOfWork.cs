using MarkMyDoctor.Models.Entities;

namespace MarkMyDoctor.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IRepository<Doctor> doctorRepo;
        private IRepository<Review> reviewRepo;
        private DoctorDbContext context;

        public UnitOfWork(DoctorDbContext context)
        {
            this.context = context;
        }

        public IDoctorRepository DoctorRepository
        {
            get
            {
                return new DoctorRepository(context);
            }
        }


        public IReviewRepository ReviewRepository
        {
            get
            {
                return new ReviewRepository(context);
            }
        }

        public void Commit()
        {
            this.context.SaveChanges();
        }

        public void Rollback()
        {
            this.context.Dispose();
        }
    }
}
