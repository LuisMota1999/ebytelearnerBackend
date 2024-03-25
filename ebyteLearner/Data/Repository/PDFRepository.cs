using AutoMapper;
using ebyteLearner.DTOs.PDF;
using ebyteLearner.Helpers;
using ebyteLearner.Models;
using Microsoft.EntityFrameworkCore;

namespace ebyteLearner.Data.Repository
{

    public interface IPDFRepository
    {
        Task<int> Create(CreatePDFRequestDTO request);
        Task<PDFDTO> ReadByModule(Guid id);
        Task<PDFDTO> ReadBySession(Guid id);
        Task<PDFDTO> Read(Guid id);
        Task<PDFDTO> Update(Guid id, UpdatePDFRequestDTO request);
        Task Delete(Guid id);
    }

    public class PDFRepository : IPDFRepository
    {
        private readonly DBContextService _dbContext;
        private readonly ILogger<PDFRepository> _logger;
        private readonly IMapper _mapper;

        public PDFRepository(DBContextService dbContext, ILogger<PDFRepository> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> Create(CreatePDFRequestDTO request)
        {
            if (request == null) throw new ArgumentNullException();

            if (_dbContext.Module.Find(request.ModuleID) == null)
                throw new AppException("Module '" + request.ModuleID + "' not found or do not exist");

            var pdf = _mapper.Map<Pdf>(request);
            pdf.PDFPath = "https://uploadthing-prod.s3.us-west-2.amazonaws.com/" + pdf.Id.ToString();
            _dbContext.Pdf.Add(pdf);
            try
            {
                // Save changes asynchronously
                var rowsAffected = await _dbContext.SaveChangesAsync();

                // Log successful creation
                _logger.LogInformation($"Created pdf with ID: {pdf.Id}, rows affected: {rowsAffected}");

                return rowsAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading pdf");

                throw;
            }
        }

        public async Task<PDFDTO> Read(Guid id)
        {

            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID.", nameof(id));
            }

            var pdfDB = await _dbContext.Pdf.FirstAsync();

            if (pdfDB != null)
            {
                var pdfResponse = _mapper.Map<PDFDTO>(pdfDB);
                return pdfResponse;
            }
            else
                throw new AppException("PDF '" + id + "' not found");
        }
        public async Task<PDFDTO> ReadByModule(Guid id)
        {

            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID.", nameof(id));
            }

            var pdfDB = await _dbContext.Pdf
                .Where(pdf => pdf.ModuleID == id)
                .FirstAsync();

            if (pdfDB != null)
            {
                var pdfResponse = _mapper.Map<PDFDTO>(pdfDB);
                return pdfResponse;
            }
            else
                throw new AppException("PDF '" + id + "' not found");
        }

        public async Task<PDFDTO> ReadBySession(Guid id)
        {
            var sessionDB = await _dbContext.Session.FindAsync(id);
            if (sessionDB != null)
            {
                var pdfDB = await _dbContext.Pdf.FindAsync(sessionDB.Id);
                var pdfResponse = _mapper.Map<PDFDTO>(pdfDB);
                return pdfResponse;
            }
            else
                throw new AppException("PDF '" + id + "' not found");
        }

        public async Task<PDFDTO> Update(Guid id, UpdatePDFRequestDTO request)
        {
            var pdfDB = await _dbContext.Pdf.FindAsync(id);
            if (pdfDB != null)
            {
                _mapper.Map(request, pdfDB);
                await _dbContext.SaveChangesAsync();
                var updatedModule = _mapper.Map<PDFDTO>(pdfDB);

                return updatedModule;
            }
            else
                throw new AppException("PDF '" + id + "' not found");
        }

        public async Task Delete(Guid id)
        {
            var pdf = await _dbContext.Pdf.FindAsync(id);
            if (pdf != null)
            {
                _dbContext.Remove(pdf);
                _dbContext.SaveChanges();
            }
            else
                throw new AppException("PDF '" + id + "' not found");
        }
    }
}
