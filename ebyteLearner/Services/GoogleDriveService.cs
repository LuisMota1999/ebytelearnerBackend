using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.IdentityModel.Tokens;
using Google.Apis.Download;
using System.ComponentModel.DataAnnotations;
using ebyteLearner.Models;
using System.IO;
using System.Net.Mail;

namespace ebyteLearner.Services
{
    public interface IDriveServiceHelper
    {
        Task<string> CreateFolder(string folderName = "", string parent = "");
        Task<string> GrantFolderPermission(string emailAddress, string userRole, string directoryId = "");
        Task<string> UploadFile(MemoryStream file, string fileName, string fileMime, string folder, string fileDescription);
        void DeleteFile(string fileId);
        IEnumerable<Google.Apis.Drive.v3.Data.File> GetFilesFromFolder(string folder = "");
        IEnumerable<Google.Apis.Drive.v3.Data.File> GetFolders();
        (MemoryStream stream, string name)? DriveDownloadFile(string fileId);
    }
    public class DriveServiceHelper : IDriveServiceHelper
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PDFService> _logger;

        public DriveServiceHelper(IConfiguration configuration, ILogger<PDFService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        private string ClientId => _configuration["ClientId"];
        private string ClientSecret => _configuration["ClientSecret"];
        private string AccessToken => _configuration["AccessToken"];
        private string RefreshToken => _configuration["RefreshToken"];
        private string DirectoryID => _configuration["DirectoryId"];
        private string Username => _configuration["Username"];

        private DriveService GetService()
        {
            var applicationName = "ebyteLearner";
            var username = Username;

            var tokenResponse = new TokenResponse
            {
                AccessToken = AccessToken,
                RefreshToken = RefreshToken
            };

            var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = ClientId,
                    ClientSecret = ClientSecret
                },
                Scopes = new[] { "https://www.googleapis.com/auth/drive" },
                DataStore = new FileDataStore(applicationName)
            });

            var credential = new UserCredential(apiCodeFlow, username, tokenResponse);

            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });
            return service;
        }

        public async Task<string> CreateFolder(string folderName = "", string parent = "")
        {
            if (parent.IsNullOrEmpty())
            {
                parent = DirectoryID;
            }

            if (folderName.IsNullOrEmpty())
            {
                throw new ValidationException($"Folder name can not be empty");

            }
            var service = GetService();
            var driveFolder = new Google.Apis.Drive.v3.Data.File();
            driveFolder.Name = folderName;
            driveFolder.MimeType = "application/vnd.google-apps.folder";
            driveFolder.Parents = new string[] { parent };
            driveFolder.Description = "File_"+DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            var command = service.Files.Create(driveFolder);
            var file = await command.ExecuteAsync();
            return file.Id;
        }

        public async Task<string> GrantFolderPermission(string emailAddress, string userRole, string directoryId = "")
        {
            if (directoryId.IsNullOrEmpty())
            {
                directoryId = DirectoryID;
            }

            if (emailAddress.IsNullOrEmpty())
            {
                throw new ValidationException($"Email address can not be empty");

            }

            if (userRole.IsNullOrEmpty())
            {
                throw new ValidationException($"User Role can not be empty");

            }
            var service = GetService();
            var requestPermission = new Google.Apis.Drive.v3.Data.Permission();
            requestPermission.Type = "user"; //"user"; "group"; "domain"; "anyone";
            requestPermission.EmailAddress = emailAddress;
            requestPermission.Role = userRole; //"reader", "writer", "owner", "organizer"
 
            var command = service.Permissions.Create(requestPermission, directoryId);
            var file = await command.ExecuteAsync();
            
            return file.Id;
        }

        public async Task<string> UploadFile(MemoryStream file, string fileName, string fileMime, string folder, string fileDescription)
        {
            if (folder.IsNullOrEmpty())
                folder = DirectoryID;

            DriveService service = GetService();

            var driveFile = new Google.Apis.Drive.v3.Data.File();
            driveFile.Name = fileName;
            driveFile.Description = fileDescription;
            driveFile.MimeType = fileMime;
            driveFile.Parents = new string[] { folder };


            var request = service.Files.Create(driveFile, file, fileMime);
            request.Fields = "id, webViewLink, thumbnailLink";

            var response = await request.UploadAsync();
            if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                throw response.Exception;

            var requestPermission = new Google.Apis.Drive.v3.Data.Permission();
            requestPermission.Type = "anyone"; //"user"; "group"; "domain"; "anyone";
            requestPermission.Role = "reader";
            var command = service.Permissions.Create(requestPermission, request.ResponseBody.Id);
            await command.ExecuteAsync();
            return request.ResponseBody.Id;
        }

        public void DeleteFile(string fileId)
        {
            var service = GetService();
            var command = service.Files.Delete(fileId);
            command.Execute();
        }

        public IEnumerable<Google.Apis.Drive.v3.Data.File> GetFilesFromFolder(string folder = "")
        {
            if (folder.IsNullOrEmpty())
                folder = DirectoryID;

            var service = GetService();

            var fileList = service.Files.List();
            fileList.Q = $"mimeType!='application/vnd.google-apps.folder' and '{folder}' in parents and trashed=false";
            fileList.Fields = "nextPageToken, files(id, name, size, mimeType, webViewLink, thumbnailLink)";

            var result = new List<Google.Apis.Drive.v3.Data.File>();
            string pageToken = null;
            do
            {
                fileList.PageToken = pageToken;
                var filesResult = fileList.Execute();
                var files = filesResult.Files;
                pageToken = filesResult.NextPageToken;
                result.AddRange(files);
            } while (pageToken != null);


            return result;
        }

        public IEnumerable<Google.Apis.Drive.v3.Data.File> GetFolders()
        {
            var service = GetService();

            var fileList = service.Files.List();
            fileList.Q = $"mimeType ='application/vnd.google-apps.folder'";
            fileList.Fields = "nextPageToken, files(id, name, size, mimeType)";

            var result = new List<Google.Apis.Drive.v3.Data.File>();
            string pageToken = null;
            do
            {
                fileList.PageToken = pageToken;
                var filesResult = fileList.Execute();
                var files = filesResult.Files;
                pageToken = filesResult.NextPageToken;
                result.AddRange(files);
            } while (pageToken != null);


            return result;
        }

        public (MemoryStream stream, string name)? DriveDownloadFile(string fileId)
        {
            try
            {

                DriveService service = GetService();

                var request = service.Files.Get(fileId);
                var name = request.Execute().Name;
                var stream = new MemoryStream();
                request.MediaDownloader.ProgressChanged +=
                    progress =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                {
                                    _logger.LogInformation($"Download progress: {progress.BytesDownloaded}");
                                    break;
                                }
                            case DownloadStatus.Completed:
                                {
                                    _logger.LogInformation("Download complete.");
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {
                                    _logger.LogInformation("Download failed.");
                                    break;
                                }
                        }
                    };
                request.Download(stream);

                return (stream, name);
            }
            catch (Exception e)
            {
                if (e is AggregateException)
                {
                    _logger.LogError($"Credential Not found");
                }
                else
                {
                    throw;
                }
            }
            return null;
        }
    }
}
