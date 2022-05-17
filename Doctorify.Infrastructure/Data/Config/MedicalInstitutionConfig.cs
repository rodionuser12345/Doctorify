using Doctorify.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctorify.Infrastructure.Data.Config;

public class MedicalInstitutionConfig : IEntityTypeConfiguration<MedicalInstitution>
{
    public void Configure(EntityTypeBuilder<MedicalInstitution> builder)
    {
        builder.Property(mi => mi.FullName)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasMany(mi => mi.Doctors)
               .WithOne(d => d.MedicalInstitution)
               .HasForeignKey(d => d.MedicalInstitutionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(o => o.RowVersion)
               .IsRowVersion();
    }
}