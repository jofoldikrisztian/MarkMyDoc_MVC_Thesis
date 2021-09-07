using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Models.Entities
{
    public class User : IEntityTypeConfiguration<User>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Review> Reviews { get; set; }


        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Name).HasMaxLength(100);
            builder.HasIndex(u => u.Name).IsUnique();
        }
    }
}
