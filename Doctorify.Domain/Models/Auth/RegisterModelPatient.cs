using System.ComponentModel.DataAnnotations;
using Doctorify.Domain.Models.Dtos;

namespace Doctorify.Domain.Models.Auth;

public class RegisterModelPatient
{
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        
        [Required(ErrorMessage = "Telephone number is required")]
        public TelephoneNumberDto? TelephoneNumber { get; set; }
        
        [Required(ErrorMessage = "Patient info is required")]
        public PatientRequestDto? Patient { get; set; }
        
        [Required(ErrorMessage = "Address is required")]
        public AddressDto? Address { get; set; }
        
}