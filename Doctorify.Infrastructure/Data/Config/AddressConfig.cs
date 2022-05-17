using Doctorify.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctorify.Infrastructure.Data.Config;

public class AddressConfig : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(a => a.Country)
               .HasColumnName("Country")
               .HasMaxLength(50)
               .IsRequired();
        
        builder.Property(a => a.City)
               .HasColumnName("City")
               .HasMaxLength(50)
               .IsRequired();
        
        builder.Property(a => a.State)
               .HasColumnName("State")
               .HasMaxLength(50)
               .IsRequired();
        
        builder.Property(a => a.Street) 
               .HasColumnName("Street")
               .HasMaxLength(50)
               .IsRequired();
        
        builder.Property(a => a.PostalCode)
               .HasColumnName("PostalCode")
               .HasMaxLength(50)
               .IsRequired();
        
        builder.HasOne(a => a.MedicalInstitution)
               .WithOne(mi => mi.Address)
               .HasForeignKey<MedicalInstitution>(mi => mi.AddressId);
        
        builder.HasMany(a => a.Patients)
               .WithOne(p => p.Address)
               .HasForeignKey(p => p.AddressId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(o => o.RowVersion)
               .IsRowVersion();
    }
}