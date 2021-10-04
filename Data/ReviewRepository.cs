using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly DoctorDbContext dbContext;

        public ReviewRepository(DoctorDbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;

        }


        public async Task<bool> CreateAsync(DoctorReviewViewModel doctorReviewViewModel, User user)
        {
            var review = doctorReviewViewModel.Review;

            review.DoctorId = doctorReviewViewModel.Doctor.Id;
            review.UserId = user.Id;
            review.ReviewedOn = DateTime.Today;


            //if (review.Doctor == null || review.User == null)
            //{
            //    return false;
            //}

            await dbContext.Reviews.AddAsync(review);

            var reviewScore = review.CommunicationRating +
                              review.EmpathyRating +
                              review.TrustAtmosphereRating +
                              review.HumanityRating +
                              review.ProfessionalismRating;

            await CalculateDoctorOverall(review.DoctorId, reviewScore);

            await dbContext.SaveChangesAsync();

            return true;
        }



        private async Task CalculateDoctorOverall(int id, int actualReviewScore)
        {
            var doc = await dbContext.Doctors.FindAsync(id);


            if (doc != null)
            {
                if (dbContext.Reviews.Where(r => r.DoctorId == id).Any())
                {
                    var professionalism = dbContext.Reviews.Where(review => review.Doctor.Id.Equals(id)).Average(review => review.ProfessionalismRating);
                    var humanity = dbContext.Reviews.Where(review => review.Doctor.Id.Equals(id)).Average(review => review.HumanityRating);
                    var communication = dbContext.Reviews.Where(review => review.Doctor.Id.Equals(id)).Average(review => review.CommunicationRating);
                    var empathy = dbContext.Reviews.Where(review => review.Doctor.Id.Equals(id)).Average(review => review.EmpathyRating);
                    var trust = dbContext.Reviews.Where(review => review.Doctor.Id.Equals(id)).Average(review => review.TrustAtmosphereRating);

                    doc.OverallRating = Convert.ToByte(Math.Round((professionalism + humanity + trust + empathy + communication + actualReviewScore) / 5.0));
                }
                else
                {
                    doc.OverallRating = Convert.ToByte(Math.Round((actualReviewScore) / 5.0));
                }

                dbContext.Doctors.Update(doc);
            }
        }

    }
}
