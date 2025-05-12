using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required]
        // Basic validation to ensure the role is one of the predefined ones.
        // You could use an Enum here and a custom validation attribute for stricter control.
        [RegularExpression("^(Employer|Employee)$", ErrorMessage = "Role must be 'Employer' or 'Employee'.")]
        public string Role { get; set; } = string.Empty; // "Employer" or "Employee"
    }
}
