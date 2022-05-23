using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Doctorify.Domain.Models.Auth
{
    public class DoctorifyUser: IdentityUser<int>
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
