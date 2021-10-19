using MarkMyDoctor.Interfaces;
using MarkMyDoctor.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;

namespace MarkMyDoctor.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        //private IRepository<Doctor> doctorRepo;
        //private IRepository<Review> reviewRepo;
        private readonly UserManager<User> userManager;
        private DoctorDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly RoleManager<IdentityRole<int>> roleManager;

        public UnitOfWork(UserManager<User> userManager, DoctorDbContext context, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole<int>> roleManager)
        {
            this.userManager = userManager;
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.roleManager = roleManager;
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
                return new ReviewRepository(userManager, context, httpContextAccessor);
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return new UserRepository(userManager, context, roleManager);
            }
        }

        public void Commit()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
