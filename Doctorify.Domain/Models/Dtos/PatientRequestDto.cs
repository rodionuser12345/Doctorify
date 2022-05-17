namespace Doctorify.Domain.Models.Dtos;

public class PatientRequestDto
{
    public string FirstName { get; set; }    
    public string LastName { get; set; }    
    public DateTime DateOfBirth { get; set; }
    public string BloodType { get; set; }
    public string Diagnose { get; set; }
    public int TelephoneId { get; set; }
    public int AddressId { get; set; }
}