namespace Doctorify.Domain.Models.Dtos;

public class MedicalInstitutionRequestDto
{
    public string FullName { get; set; }
    public int AddressId { get; set; }
    public int TelephoneNumberId { get; set; }
}