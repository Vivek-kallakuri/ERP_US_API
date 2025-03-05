using System.ComponentModel.DataAnnotations;

namespace Cortracker360_Accurate_API.Models
{
    public class SignupModel
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
