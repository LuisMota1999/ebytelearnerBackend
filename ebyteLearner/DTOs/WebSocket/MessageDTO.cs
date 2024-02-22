using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.WebSocket
{
    public class MessageDTO
    {
        [Required]
        public string Message { get; set; }
    }
}
