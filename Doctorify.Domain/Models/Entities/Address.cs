namespace Doctorify.Domain.Models.Entities;

public class Address : BaseEntity
{
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    
    public virtual MedicalInstitution MedicalInstitution { get; set; }
    
    public virtual IList<Patient> Patients { get; set; }
    
    public byte[] RowVersion { get; set; }
} 