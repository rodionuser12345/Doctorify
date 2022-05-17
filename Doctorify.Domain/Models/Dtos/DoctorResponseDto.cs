namespace Doctorify.Domain.Models.Dtos;

public class DoctorResponseDto
{
    public string FullName { get; set; }
    public string Specialization { get; set; }
    public DateTime DateOfBirth { get; set; }
    public TelephoneNumberDto TelephoneNumber { get; set; }
    public MedicalInstitutionResponseDto MedicalInstitution { get; set; }
}