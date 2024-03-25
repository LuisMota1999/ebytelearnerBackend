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
        public Guid SessionPdfId { get; set; }
        [Required]
        public Guid SessionModuleID { get; set; }
        [Required]
        public DateTimeOffset StartSessionDate { get; set; } = DateTimeOffset.UtcNow;
        [Required]
        public DateTimeOffset EndSessionDate { get; set; } = DateTimeOffset.UtcNow.AddHours(1);
        public DateTimeOffset CreatedDate { get; init; } = DateTimeOffset.UtcNow;
    }
}
