using MarkMyDoctor.Data;
using MarkMyDoctor.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarkMyDoctor.Models
{
    public static class Seed
    {

        public async static void Data(IApplicationBuilder app)
        {
            using (var dbContext = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<DoctorDbContext>())
            {
                CheckMigrations(dbContext);

                if (!dbContext.Cities.Any())
                {
                    dbContext.Cities.AddRange(ReadCities());
                }

                await dbContext.SaveChangesAsync();


                if (!dbContext.Facilities.Any())
                {
                    dbContext.Facilities.AddRange(ReadFacilities(dbContext));
                }

                if (!dbContext.Doctors.Any())
                {
                    dbContext.Doctors.AddRange(ReadDoctors());
                }

                if (!dbContext.Specialities.Any())
                {
                    dbContext.Specialities.AddRange(ReadSpecialities(dbContext));
                }

                await dbContext.SaveChangesAsync();


                if (!dbContext.DoctorSpecialities.Any())
                {
                    dbContext.DoctorSpecialities.AddRange(AddSpecialitiesToDoctors(dbContext));
                }

                if (!dbContext.DoctorFacilities.Any())
                {
                    dbContext.DoctorFacilities.AddRange(AddFacilitiesToDoctors(dbContext));
                }

                if (!dbContext.Users.Any())
                {
                    dbContext.Users.Add(new User() { Name = "Gyuri" });
                }

                await dbContext.SaveChangesAsync();
            }
        }

        private static IEnumerable<DoctorFacility> AddFacilitiesToDoctors(DoctorDbContext dbContext)
        {
            var docFacList = new List<DoctorFacility>();

            foreach (var fac in File.ReadAllLines(@"./PopulateData/housedoctors.txt"))
            {
                var line = fac.Split("\t");

                if (line[3] != "BETÖLTETLEN")
                {
                    var docFac = new DoctorFacility()
                    {
                        DoctorId = dbContext.Doctors.First(d => d.Name.Equals(line[3]) && d.PhoneNumber.Equals(line[2])).Id,
                        FacilityId = dbContext.Facilities.First(f => f.Address.Equals(line[1])).Id
                    };
                    docFacList.Add(docFac);
                }
            }
            return docFacList;
        }

        private static IEnumerable<DoctorSpeciality> AddSpecialitiesToDoctors(DoctorDbContext dbContext)
        {
            var docSpecList = new List<DoctorSpeciality>();

            foreach (var doc in dbContext.Doctors)
            {
                var docSpec = new DoctorSpeciality()
                {
                    DoctorId = doc.Id,
                    SpecialityId = dbContext.Specialities.First(s => s.Name.Equals("háziorvos")).Id
                };

                docSpecList.Add(docSpec);
            }

            return docSpecList;

        }

        private static IEnumerable<Facility> ReadFacilities(DoctorDbContext db)
        {

            var facilities = new List<Facility>();

            foreach (var fac in File.ReadAllLines(@"./PopulateData/housedoctors.txt"))
            {
                var line = fac.Split("\t");

                var facility = new Facility()
                {
                    Name = line[0] + " " + line[1] + "-i háziorvosi rendelő",
                    City = db.Cities.FirstOrDefault(c => c.Name.Equals(line[0])) ?? db.Cities.First(c => c.Name.Equals("Ismeretlen")),
                    Address = line[1]
                };

                if (!facilities.Any(f => f.Name.Equals(facility.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    facilities.Add(facility);
                }
            }

            return facilities;
        }


        private static IEnumerable<Speciality> ReadSpecialities(object context)
        {
            return File.ReadAllLines(@"./PopulateData/specialities.txt").Select(speciality => new Speciality() { Name = speciality.ToLower() }).ToList();
        }

        private static IEnumerable<Doctor> ReadDoctors()
        {
            var doctors = new List<Doctor>();

            foreach (var doctor in File.ReadAllLines(@"./PopulateData/housedoctors.txt"))
            {
                var doctorLine = doctor.Split("\t");

                var currentDoctor = new Doctor()
                {
                    Name = doctorLine[3],
                    PhoneNumber = doctorLine[2],
                    CanPayWithCard = false,
                    Email = "<Ismeretlen>",
                    WebAddress = "<Ismeretlen>"
                };

                if (!doctors.Any(d => d.Name == currentDoctor.Name && d.PhoneNumber == doctorLine[2]))
                {
                    if (currentDoctor.Name != "BETÖLTETLEN")
                    {
                        doctors.Add(currentDoctor);
                    }
                }
            }
            return doctors;
        }

        private static IEnumerable<City> ReadCities()
        {
            return File.ReadAllLines(@"./PopulateData/cities.txt").Select(line => new City() { Name = line }).ToList();
        }

        private static void CheckMigrations(DoctorDbContext context)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
