using System.ComponentModel.DataAnnotations;
using Doctorify.Domain.Models.Dtos;

namespace Doctorify.Domain.Models.Auth;

public class RegisterModelDoctor
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
        
    [Required(ErrorMessage = "Specialization is required")]
    public string? Specialization { get; set; }
        
    [Required(ErrorMessage = "Telephone number is required")]
    public TelephoneNumberDto? TelephoneNumber { get; set; }
        
    [Required(ErrorMessage = "MedicalInstitution is required")]
    public long? MedicalInstitutionId { get; set; }
}