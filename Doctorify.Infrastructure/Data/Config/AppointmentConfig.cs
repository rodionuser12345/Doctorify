using Doctorify.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doctorify.Infrastructure.Data.Config;

public class AppointmentConfig : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.Property(a => a.Description)
               .HasColumnName("Description")
               .HasMaxLength(200)
               .IsRequired();
        
        builder.Property(a => a.Date)
               .HasColumnName("Date")
               .HasColumnType("datetime")
               .IsRequired();
        
        builder.Property(a => a.AppointmentStatus)
               .HasColumnName("AppointmentStatus")
               .HasMaxLength(20)
               .HasConversion<string>()
               .IsRequired();

        builder.HasOne(a => a.Doctor)
               .WithMany(d => d.Appointments)
               .HasForeignKey(a => a.DoctorId);
        
        builder.HasOne(a => a.Patient)
               .WithMany(p => p.Appointments)
               .HasForeignKey(a => a.PatientId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.Property(o => o.RowVersion)
               .IsRowVersion();

    }
}