using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Auth
{
    public class RegisterRequestDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
