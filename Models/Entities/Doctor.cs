using MarkMyDoctor.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarkMyDoctor.Models.Entities
{
    public class Doctor : IEntityTypeConfiguration<Doctor>, IEntity
    {
        public int Id { get; set; }
        public bool IsStartWithDr { get; set; }
        [Required(ErrorMessage = "A név megadása kötelező!")]
        [Display(Name = "Név:")]
        public string Name { get; set; }
        [Required(ErrorMessage = "A telefonszám megadása kötelező!")]
        [Display(Name = "Telefonszám:")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Bakkártyás fizetés lehetséges?")]
        public bool CanPayWithCard { get; set; }
        [Display(Name = "E-mail cím:")]
        public string? Email { get; set; }
        [Display(Name = "Weblap:")]
        public string? WebAddress { get; set; }
        [Display(Name = "Profilkép:")]
        public byte[]? ProfilePicture { get; set; }
        public double? OverallRating { get; set; }

        public ICollection<Review>? Reviews { get; set; }
        public ICollection<DoctorFacility>? DoctorFacilities { get; set; }
        public ICollection<DoctorSpeciality>? DoctorSpecialities { get; set; }

        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(d => d.Name).HasMaxLength(50);
        }
    }
}
