using System.ComponentModel.DataAnnotations.Schema;

namespace Doctorify.Domain.Models.Entities;

public class MedicalInstitution : BaseEntity
{
    public string FullName { get; set; }
    
    public long AddressId { get; set; }
    public virtual Address Address { get; set; }
    
    public long TelephoneNumberId { get; set; }
    public virtual TelephoneNumber TelephoneNumber { get; set; }
    
    public virtual List<Doctor> Doctors { get; set; }
    
    public byte[] RowVersion { get; set; }
}