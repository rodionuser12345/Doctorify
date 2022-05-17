namespace Doctorify.Domain.Models.Entities;

public class TelephoneNumber : BaseEntity
{
    public string Home { get; set; }
    public string Work { get; set; }
    
    public virtual Doctor Doctor { get; set; }
    public virtual MedicalInstitution MedicalInstitution { get; set; }
    public virtual Patient Patient { get; set; }
    
    public byte[] RowVersion { get; set; }
}