using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Auth
{
    public class AuthRequestDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
