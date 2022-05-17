using System.ComponentModel.DataAnnotations.Schema;
using Doctorify.Domain.Models.Enums;

namespace Doctorify.Domain.Models.Entities;

public class Patient : BaseEntity
{
    public string FirstName { get; set; }    
    public string LastName { get; set; }    
    public DateTime DateOfBirth { get; set; }
    public BloodType BloodType { get; set; }
    public string Diagnose { get; set; }
    
    public long TelephoneNumberId { get; set; }
    public virtual TelephoneNumber TelephoneNumber { get; set; }
    
    public long AddressId { get; set; }
    public virtual Address Address { get; set; }
    
    public virtual IList<Doctor> Doctors { get; set; }
    
    public virtual IList<Appointment> Appointments { get; set; }
    
    public byte[] RowVersion { get; set; }
}