using Doctorify.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctorify.Infrastructure.Data.Config;

public class TelephoneNumberConfig : IEntityTypeConfiguration<TelephoneNumber>
{
    public void Configure(EntityTypeBuilder<TelephoneNumber> builder)
    {
        builder.Property(t => t.Number)
               .HasColumnName("Number")
               .HasMaxLength(8)
               .IsRequired();

        builder.HasOne(t => t.Doctor)
               .WithOne(d => d.TelephoneNumber)
               .HasForeignKey<Doctor>(d => d.TelephoneNumberId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(t => t.MedicalInstitution)
               .WithOne(d => d.TelephoneNumber)
               .HasForeignKey<MedicalInstitution>(d => d.TelephoneNumberId);

        builder.HasOne(t => t.Patient)
               .WithOne(d => d.TelephoneNumber)
               .HasForeignKey<Patient>(d => d.TelephoneNumberId);

        builder.Property(o => o.RowVersion)
               .IsRowVersion();
    }
}