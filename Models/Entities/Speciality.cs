using MarkMyDoctor.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace MarkMyDoctor.Models.Entities
{
    public class Speciality : IEntityTypeConfiguration<Speciality>, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<DoctorSpeciality>? DoctorSpecialities { get; set; }

        public void Configure(EntityTypeBuilder<Speciality> builder)
        {
            builder.Property(s => s.Name).HasMaxLength(40);
            builder.HasIndex(s => s.Name).IsUnique();
        }
    }
}