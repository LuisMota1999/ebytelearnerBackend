using ebyteLearner.DTOs.Module;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.Module
{
    public class CreateSessionRequestDTO
    {
        [Required]
        public string SessionName { get; set; }
        public string SessionDescription { get; set; }
        [Required]
        public byte[] QRCode { get; set; }
        [Required]
        public Guid ModuleID { get; set; }
        [Required]
        public DateTimeOffset StartSessionDate { get; set; }
        [Required]
        public DateTimeOffset EndSessionDate { get; set; }
        public DateTimeOffset CreatedDate { get; init; } = DateTimeOffset.Now;
    }
}
