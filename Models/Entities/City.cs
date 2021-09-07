using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace MarkMyDoctor.Models.Entities
{
    public class City : IEntityTypeConfiguration<City>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Facility> Facilities { get; set; }

        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.Property(c => c.Name).HasMaxLength(30);
            builder.HasIndex(c => c.Name).IsUnique();
        }
    }
}