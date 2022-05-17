using System.ComponentModel.DataAnnotations.Schema;
using Doctorify.Domain.Models.Enums;

namespace Doctorify.Domain.Models.Entities;

public class Doctor : BaseEntity
{
    public string Prefix => "Dr.";
    public string FirstName { get; set; }    
    public string LastName { get; set; }    
    public DoctorSpecialization Specialization { get; set; }
    public DateTime DateOfBirth { get; set; }

    public long TelephoneNumberId { get; set; }
    public virtual TelephoneNumber TelephoneNumber { get; set; }
    
    public long MedicalInstitutionId { get; set; }
    public virtual MedicalInstitution MedicalInstitution { get; set; }
    
    public virtual IList<Patient> Patients { get; set; }
    
    public virtual IList<Appointment> Appointments { get; set; }

    public byte[] RowVersion { get; set; }
}