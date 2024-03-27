using AutoMapper;
using ebyteLearner.Data.Repository;
using ebyteLearner.Helpers;
using ebyteLearner.Models;
using ebyteLearner.DTOs.PDF;
using System.ComponentModel.DataAnnotations;
using ebyteLearner.DTOs.Module;

namespace ebyteLearner.Services
{
    public interface IPDFService
    {
        Task<PDFDTO> DownloadPDF(Guid id);
        Task<int> UploadPDF(string base64, int numberPages, string fileName, Guid moduleId, long contentLength, string contentType, MemoryStream file);
        Task<PDFDTO> GetPDFBySessionID(Guid id);
        Task<PDFDTO> GetPDFByModuleID(Guid id);
    }
    public class PDFService : IPDFService
    {

        private readonly IPDFRepository _pdfRepository;
        private readonly IModuleService _moduleService;
        private readonly ILogger<PDFService> _logger;
        private readonly IMapper _mapper;
        private readonly IDriveServiceHelper _driveService;
        private readonly ICacheService _cacheService;

        public PDFService(IPDFRepository pdfRepository, ILogger<PDFService> logger, IMapper mapper, IDriveServiceHelper driveService, IModuleService moduleService, ICacheService cacheService)
        {
            _pdfRepository = pdfRepository;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _driveService = driveService;
            _moduleService = moduleService;
            _cacheService = cacheService;
        }

        public async Task<PDFDTO> DownloadPDF(Guid id)
        {
            if (id == Guid.Empty) { throw new ArgumentNullException(nameof(id)); }

            var cachedPdf = _cacheService.GetData<PDFDTO>(id.ToString());
            if (cachedPdf != null)
                return cachedPdf;

            var expiryTime = DateTimeOffset.Now.AddMinutes(60);

            var response = await _pdfRepository.Read(id);
            _cacheService.SetData<PDFDTO>(id.ToString(), response, expiryTime);
            return response;
        }

        public async Task<int> UploadPDF(string base64, int numberPages, string fileName, Guid moduleId, long contentLength, string contentType, MemoryStream file)
        {
            try
            {
                if (contentLength <= 0 || file.Length <= 0)
                {
                    _logger.LogError("Uploading file to Google Drive failed. Can not upload 0 byte file to drive.");
                    throw new ValidationException();
                }

                var module = _moduleService.GetModule(moduleId);
                if (module == null)
                {
                    _logger.LogError($"Module {moduleId} not found");
                    throw new ValidationException($"Module {moduleId} not found");
                }

                var pdfpath = await _driveService.UploadFile(file, fileName, contentType, "", moduleId.ToString());

                _logger.LogInformation("Uploading file to Google Drive complete.");

                var pdf = new Pdf();
                pdf.PDFNumberPages = numberPages;
                pdf.PDFContent = base64;
                pdf.PDFName = fileName;
                pdf.ModuleID = moduleId;
                pdf.PDFLength = contentLength;
                pdf.PDFPath = pdfpath;

                var pdfRequest = _mapper.Map<CreatePDFRequestDTO>(pdf);

                return await _pdfRepository.Create(pdfRequest);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public async Task<PDFDTO> GetPDFBySessionID(Guid id)
        {
            return await _pdfRepository.ReadByModule(id); ;
        }

        public async Task<PDFDTO> GetPDFByModuleID(Guid id)
        {   
            return await _pdfRepository.ReadByModule(id); ;
        }
    }
}
