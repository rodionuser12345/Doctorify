using System.Reflection;
using Doctorify.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Doctorify.Infrastructure.Data.Context;

public class DoctorifyContext : DbContext
{
    public DoctorifyContext(DbContextOptions<DoctorifyContext> options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<MedicalInstitution> MedicalInstitutions { get; set; }
    public DbSet<TelephoneNumber> TelephoneNumbers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<TelephoneNumber>()
                    .Property(t => t.RowVersion).IsRowVersion();
        
        modelBuilder.Entity<Address>()
                    .Property(a => a.RowVersion).IsRowVersion();
        
        modelBuilder.Entity<MedicalInstitution>()
                    .Property(m => m.RowVersion).IsRowVersion();
        
        modelBuilder.Entity<Patient>()
                    .Property(p => p.RowVersion).IsRowVersion();
        
        modelBuilder.Entity<Doctor>()
                    .Property(d => d.RowVersion).IsRowVersion();
        
        modelBuilder.Entity<Appointment>()
                    .Property(d => d.RowVersion).IsRowVersion();

        modelBuilder.Entity<Patient>()
                    .HasMany(p => p.Doctors)
                    .WithMany(d => d.Patients)
                    .UsingEntity(e => e.ToTable("DoctorsPatients"));
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}