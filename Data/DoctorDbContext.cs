using MarkMyDoctor.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MarkMyDoctor.Data
{
    public class DoctorDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DoctorDbContext(DbContextOptions<DoctorDbContext> options) : base(options) { }


        public DbSet<City> Cities { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorFacility> DoctorFacilities { get; set; }
        public DbSet<DoctorSpeciality> DoctorSpecialities { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Speciality> Specialities { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            //Megkeresi az összes olyan objektumot, mely megvalósítja IEntityTypeConfiguration interfészt, és meghívja a Configure metódusát.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Doctor>().ToTable("Doctors");
            modelBuilder.Entity<City>().ToTable("Cities");
            modelBuilder.Entity<DoctorFacility>().ToTable("DoctorFacilities");
            modelBuilder.Entity<DoctorSpeciality>().ToTable("DoctorSpeciality");
            modelBuilder.Entity<Facility>().ToTable("Facilities");
            modelBuilder.Entity<Review>().ToTable("Reviews");
            modelBuilder.Entity<Speciality>().ToTable("Specialities");
            modelBuilder.Entity<User>().ToTable("Users");

        }



    }
}
