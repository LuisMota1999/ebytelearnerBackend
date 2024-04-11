using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.GoogleDrive
{
    public class GoogleDriveFileResponseDTO
    {
        public string FileName { get; set; }
        public string FileId { get; set; }
        public string FileContent { get; set; }
    }
}
