using Doctorify.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctorify.Infrastructure.Data.Config;

public class DoctorConfig : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        
        builder.Property(d => d.FirstName)
               .HasColumnName("FirstName")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(d => d.LastName)
               .HasColumnName("LastName")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(d => d.DateOfBirth)
               .HasColumnName("DateOfBirth")
               .HasColumnType("date")
               .IsRequired();
        
        builder.Property(d => d.Specialization)
               .HasColumnName("Specialization")
               .HasMaxLength(50)
               .HasConversion<string>()
               .IsRequired();

        builder.Property(o => o.RowVersion)
               .IsRowVersion();
    }
}