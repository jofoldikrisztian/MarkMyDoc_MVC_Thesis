using MarkMyDoctor.Models;
using MarkMyDoctor.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MarkMyDoctor.Data
{
    public class DoctorDbContext : DbContext
    {
        public DoctorDbContext(DbContextOptions<DoctorDbContext> options) : base(options) { }


        public DbSet<City> Cities { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorFacility> DoctorFacilities { get; set; }
        public DbSet<DoctorSpeciality> DoctorSpecialities { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
