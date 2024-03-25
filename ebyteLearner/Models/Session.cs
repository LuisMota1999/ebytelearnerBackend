using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Models
{
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public string SessionName { get; set; }
        public string SessionDescription { get; set; }
        public byte[] QRCode { get; set; }
        public ICollection<UserSession> UserSessions { get; set; }

        [ForeignKey("SessionPdfId")]
        public Pdf SessionPdf { get; set; }
        public Guid? SessionPdfId{ get; set; }
        public Guid SessionModuleID { get; set; }

        [ForeignKey("SessionMonitoringID")]
        public SessionMonitoring? SessionMonitoring { get; set; }
        public Guid? SessionMonitoringID { get; set; }
        public DateTimeOffset StartSessionDate { get; set; }
        public DateTimeOffset EndSessionDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.Now;
    }
}
