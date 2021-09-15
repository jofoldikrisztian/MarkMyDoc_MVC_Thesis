using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Models.Entities
{
    public class User : IdentityUser<int> ,IEntityTypeConfiguration<User>
    {
        public ICollection<Review>? Reviews { get; set; }


        public void Configure(EntityTypeBuilder<User> builder)
        {
           
        }
    }
}
