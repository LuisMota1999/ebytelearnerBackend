namespace ebyteLearner.DTOs.Auth
{
    public class RegisterResponseDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string NIF { get; set; }
    }
}
