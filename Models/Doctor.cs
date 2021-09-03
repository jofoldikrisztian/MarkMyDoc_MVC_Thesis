using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Models
{
    public class Doctor : IEntityTypeConfiguration<Doctor>
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool CanPayWithCard { get; set; }
        public string? Email { get; set; }
        public string? WebAddress { get; set; }
        public byte[]? PorfilePicture { get; set; }
        public byte? OverallRating { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<DoctorFacility> DoctorFacilities { get; set; }
        public ICollection<DoctorSpeciality> DoctorSpecialities { get; set; }

        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(d => d.Name).HasMaxLength(30);
        }
    }
}
