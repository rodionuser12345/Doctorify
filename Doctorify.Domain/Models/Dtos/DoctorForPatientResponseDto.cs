namespace Doctorify.Domain.Models.Dtos;

public class DoctorForPatientResponseDto
{
    public string FullName { get; set; }
    public string Specialization { get; set; }
    public TelephoneNumberDto TelephoneNumber { get; set; }
    public string MedicalInstitution { get; set; }
}