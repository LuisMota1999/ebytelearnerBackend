using ebyteLearner.Models;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.User
{
    public class UpdateUserRequestDTO
    {
        public string? NIF { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ZipCode { get; set; }
        public string? Docn { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public DateTimeOffset? Birthday { get; set; }
    }
}
