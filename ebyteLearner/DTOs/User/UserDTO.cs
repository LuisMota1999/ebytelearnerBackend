namespace ebyteLearner.Models
{
    public class UserDTO
    {
        public Guid Id { get; init; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ZipCode { get; set; }
        public string NIF { get; set; }
        public string? Docn { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public bool Active { get; set; } = true;
        public DateTimeOffset? Birthday { get; set; }
    }
}