﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkMyDoctor.Models
{
    public class DoctorSpeciality : IEntityTypeConfiguration<DoctorSpeciality>
    {
        public int Id { get; set; }
        public int SpecialityId { get; set; }
        public Speciality Speciality { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }


        public void Configure(EntityTypeBuilder<DoctorSpeciality> builder)
        {
            builder.HasOne(ds => ds.Doctor).WithMany(d => d.DoctorSpecialities)
                .HasForeignKey(ds => ds.DoctorId).HasPrincipalKey(d => d.Id);

            builder.HasOne(ds => ds.Speciality).WithMany(d => d.DoctorSpecialities)
              .HasForeignKey(ds => ds.SpecialityId).HasPrincipalKey(s => s.Id);
        }
    }
}
