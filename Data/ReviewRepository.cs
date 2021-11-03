using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models.Entities;
using MarkMyDoctor.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly UserManager<User> userManager;
        private readonly DoctorDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ReviewRepository(UserManager<User> userManager, DoctorDbContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }


        public async Task CreateAsync(DoctorReviewViewModel doctorReviewViewModel)
        {
            var review = doctorReviewViewModel.Review;

            var loggedUser = httpContextAccessor.HttpContext?.User;

            var user = await userManager.GetUserAsync(loggedUser);

            review.DoctorId = doctorReviewViewModel.Doctor.Id;
            review.UserId = user.Id;
            review.ReviewedOn = DateTime.Today;
            review.IsApproved = false;


            await dbContext.Reviews.AddAsync(review);

            var reviewScore = (review.ProfessionalismRating * 1.4) +
                              (review.CommunicationRating * 1.3) +
                              (review.EmpathyRating * 1.2) +
                              (review.TrustAtmosphereRating * 1.1) +
                              review.HumanityRating;                              

            await CalculateDoctorOverall(review.DoctorId, reviewScore);

        }



        private async Task CalculateDoctorOverall(int id, double actualReviewScore)
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

                    doc.OverallRating = (((professionalism * 1.4) +
                                          (communication * 1.3) +
                                          (empathy * 1.2) +
                                          (trust * 1.1)+ 
                                          humanity +
                                          (actualReviewScore / 6.0)) / 7.0);

                }
                else
                {
                    doc.OverallRating = actualReviewScore / 6.0;
                }

                dbContext.Doctors.Update(doc);
            }
        }

        public async Task<IEnumerable<UnApprovedReviewViewModel>> GetUnApprovedReviewsAsync()
        {
            var reviews = await dbContext.Reviews.Where(r => r.IsApproved == false).Include( r => r.User).Include(r => r.Doctor).ToListAsync();

            var unAprrovedReviews = new List<UnApprovedReviewViewModel>();

            foreach (Review review in reviews)
            {
                var thisViewModel = new UnApprovedReviewViewModel
                {
                    Id = review.Id,
                    UserName = review.User.UserName,
                    Title = review.Title,
                    Doctor = review.Doctor.IsStartWithDr == true ? $"dr. {review.Doctor.Name}" : review.Doctor.Name,
                };
                unAprrovedReviews.Add(thisViewModel);
            }

            return unAprrovedReviews;
        }

        public async Task ApproveReviewAsync(int id)
        {
            var review = await dbContext.Reviews.FindAsync(id);

            review.IsApproved = true;

        }

    }
}
