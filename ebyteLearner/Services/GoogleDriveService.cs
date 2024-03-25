﻿using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.IdentityModel.Tokens;
using Google.Apis.Download;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Services
{
    public interface IDriveServiceHelper
    {
        string CreateFolder(string folderName = "", string parent = "");
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

        public string CreateFolder(string folderName = "", string parent = "")
        {
            if (parent.IsNullOrEmpty())
            {
                throw new ValidationException(nameof(parent));
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
            var command = service.Files.Create(driveFolder);
            var file = command.Execute();
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
            request.Fields = "id, webViewLink";

            var response = await request.UploadAsync();
            if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                throw response.Exception;

            return request.ResponseBody.ThumbnailLink;
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
