using ebyteLearner.Models;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public UserDTO User { get; set; }
        public string AccessToken { get; set; }
    }
}
