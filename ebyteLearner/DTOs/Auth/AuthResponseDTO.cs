using ebyteLearner.Models;

namespace ebyteLearner.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public UserDTO User { get; set; }
        public string AccessToken { get; set; }
        public string Role {  get; set; }
        
    }
}
