using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Models.Entities
{
    public class Review : IEntityTypeConfiguration<Review>
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "A mező kitöltése kötelező")]
        public string Title { get; set; }
        [Required(ErrorMessage = "A mező kitöltése kötelező")]
        public string ReviewBody { get; set; }
        public bool Recommend { get; set; }
        public DateTime ReviewedOn { get; set; }
        public bool IsReported { get; set; }
        public byte ProfessionalismRating { get; set; }
        public byte HumanityRating { get; set; }
        public byte CommunicationRating { get; set; }
        public byte EmpathyRating { get; set; }
        public byte FelxibilityRating { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasOne(r => r.Doctor).WithMany(d => d.Reviews)
                .HasForeignKey(r => r.DoctorId).HasPrincipalKey(d => d.Id);

            builder.HasOne(r => r.User).WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId).HasPrincipalKey(u => u.Id);
        }
    }
}
