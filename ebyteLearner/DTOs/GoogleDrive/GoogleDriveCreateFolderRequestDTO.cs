using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.GoogleDrive
{
    public class GoogleDriveCreateFolderRequestDTO
    {
        [Required]
        public string FolderName { get; set; }
    }
}
