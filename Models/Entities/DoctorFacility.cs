using MarkMyDoctor.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarkMyDoctor.Models.Entities
{
    public class DoctorFacility : IEntityTypeConfiguration<DoctorFacility>, IEntity
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

            //Egy intézmény nem tartozhat többször egy orvoshoz, és egy intézménynek nem lehet többször ugyan az az orvosa.
            builder.HasIndex(df => new { df.DoctorId, df.FacilityId }).IsUnique();
        }
    }
}
