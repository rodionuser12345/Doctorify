namespace Doctorify.Domain.Models.Dtos;

public class DoctorRequestDto
{
    public string FirstName { get; set; }    
    public string LastName { get; set; }    
    public string Specialization { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int TelephoneNumberId { get; set; }
    public int MedicalInstitutionId { get; set; }
}