using MarkMyDoctor.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace MarkMyDoctor.Models.Entities
{
    public class Facility : IEntityTypeConfiguration<Facility>, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
        public ICollection<DoctorFacility>? DoctorFacilities { get; set; }

        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.HasOne(f => f.City).WithMany(c => c.Facilities)
                .HasForeignKey(v => v.CityId).HasPrincipalKey(c => c.Id);

            builder.Property(f => f.Name).HasMaxLength(150);
            builder.HasIndex(f => f.Name).IsUnique();
        }
    }
}
