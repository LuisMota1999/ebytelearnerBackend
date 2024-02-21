using AutoMapper;
using ebyteLearner.DTOs.Module;
using ebyteLearner.DTOs.NewFolder;
using ebyteLearner.DTOs.PDF;
using ebyteLearner.Helpers;
using ebyteLearner.Models;

namespace ebyteLearner.Data.Repository
{

    public interface IPDFRepository
    {
        Task Create(CreatePDFRequestDTO request);
        Task<PDFDTO> Read(Guid id);
        Task<PDFDTO> Update(Guid id, UpdatePDFRequestDTO request);
        Task Delete(Guid id);
    }


    public class PDFRepository: IPDFRepository
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

        public async Task Create(CreatePDFRequestDTO request)
        {
            if (_dbContext.Module.Find(request.ModuleID) == null)
                throw new AppException("Module '" + request.ModuleID + "' not found or do not exist");

            var pdf = _mapper.Map<Pdf>(request);

            _dbContext.Pdf.Add(pdf);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PDFDTO> Read(Guid id)
        {
            var pdfDB = await _dbContext.Pdf.FindAsync(id);
            if (pdfDB != null)
            {
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
