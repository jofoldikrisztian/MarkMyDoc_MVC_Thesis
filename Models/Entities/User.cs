using MarkMyDoctor.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;


namespace MarkMyDoctor.Models.Entities
{
    public class User : IdentityUser<int>, IEntityTypeConfiguration<User>, IEntity
    {
        public ICollection<Review>? Reviews { get; set; }


        public void Configure(EntityTypeBuilder<User> builder)
        {

        }
    }
}
