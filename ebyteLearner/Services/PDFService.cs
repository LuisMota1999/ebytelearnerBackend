using AutoMapper;
<<<<<<< HEAD
using ebyteLearner.Data.Repository;
using ebyteLearner.Helpers;
using ebyteLearner.Models;
using ebyteLearner.DTOs.PDF;
=======
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using ebyteLearner.Data.Repository;
using ebyteLearner.DTOs.NewFolder;
using ebyteLearner.Helpers;
using ebyteLearner.Models;
>>>>>>> 1cc7beb858b6f00a4c42bcf22f5b24eba0894ee5


namespace ebyteLearner.Services
{
    public interface IPDFService
    {
        Task DownloadPDF(Guid id);
        Task<bool> UploadPDF(string base64, int numberPages, string fileName, Guid moduleId, long contentLength);
    }
    public class PDFService : IPDFService
    {

        private readonly IPDFRepository _pdfRepository;
        private readonly ILogger<PDFService> _logger;
        private readonly IMapper _mapper;

        public PDFService(IPDFRepository pdfRepository, ILogger<PDFService> logger, IMapper mapper)
        {
            _pdfRepository = pdfRepository;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public Task DownloadPDF(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UploadPDF(string base64, int numberPages, string fileName, Guid moduleId, long contentLength)
        {
            try
            {
                var pdf = new Pdf();

                pdf.PDFNumberPages = numberPages; 
                pdf.PDFContent = base64;
                pdf.PDFName = fileName;
                pdf.ModuleID = moduleId;
                pdf.PDFLength = contentLength;
                var pdfRequest = _mapper.Map<CreatePDFRequestDTO>(pdf);

                await _pdfRepository.Create(pdfRequest);
                return true;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }
    }
}
