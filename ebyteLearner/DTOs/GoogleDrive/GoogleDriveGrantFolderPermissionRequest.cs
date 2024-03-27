using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.DTOs.GoogleDrive
{
    public class GoogleDriveGrantFolderPermissionRequest
    {
        [Required]
        public string UserRole { get; set; }

        [Required]
        public string EmailAddress { get; set; }
    }
}
