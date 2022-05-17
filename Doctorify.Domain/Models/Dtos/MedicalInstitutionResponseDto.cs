namespace Doctorify.Domain.Models.Dtos;

public class MedicalInstitutionResponseDto
{
    public string FullName { get; set; }
    public AddressDto Address { get; set; }
    public TelephoneNumberDto TelephoneNumber { get; set; }
}