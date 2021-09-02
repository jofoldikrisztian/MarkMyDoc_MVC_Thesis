using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Models
{
    public class DoctorFacility : IEntityTypeConfiguration<DoctorFacility>
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int FacilityId { get; set; }
        public Facility Facility { get; set; }

        public void Configure(EntityTypeBuilder<DoctorFacility> builder)
        {
            builder.HasOne(df => df.Doctor).WithMany(doc => doc.DoctorFacilities)
                .HasForeignKey(df => df.DoctorId).HasPrincipalKey(doc => doc.Id);

            builder.HasOne(df => df.Facility).WithMany(f => f.DoctorFacilities)
                .HasForeignKey(df => df.FacilityId).HasPrincipalKey(f => f.Id);
        }
    }
}
