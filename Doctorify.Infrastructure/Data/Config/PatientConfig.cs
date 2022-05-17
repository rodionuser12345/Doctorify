using Doctorify.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctorify.Infrastructure.Data.Config;

public class PatientConfig : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.Property(p => p.FirstName)
               .HasColumnName("FirstName")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(p => p.LastName)
               .HasColumnName("LastName")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(p => p.DateOfBirth)
               .HasColumnName("DateOfBirth")
               .HasColumnType("date")
               .IsRequired();
        
        builder.Property(p => p.BloodType)
               .HasColumnName("BloodType")
               .HasMaxLength(15)
               .HasConversion<string>()
               .IsRequired();
        
        builder.Property(p => p.Diagnose)
               .HasColumnName("Diagnose")
               .HasMaxLength(250)
               .IsRequired();

        builder.Property(o => o.RowVersion)
               .IsRowVersion();
    }
}