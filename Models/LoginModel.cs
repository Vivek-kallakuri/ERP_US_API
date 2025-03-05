using System.ComponentModel.DataAnnotations;

namespace Cortracker360_Accurate_API.Models
{
    public class LoginModel
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
