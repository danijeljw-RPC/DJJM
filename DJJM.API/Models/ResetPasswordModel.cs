using System.ComponentModel.DataAnnotations;

namespace DJJM.API.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
