namespace Doctorify.Domain.Models.Dtos;

public class PatientResponseDto
{
    public string FirstName { get; set; }    
    public string LastName { get; set; }    
    public DateTime DateOfBirth { get; set; }
    public string BloodType { get; set; }
    public string Diagnose { get; set; }
    public TelephoneNumberDto Telephone { get; set; }
    public IList<DoctorForPatientResponseDto> Doctors { get; set; }
}