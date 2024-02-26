using AutoMapper;
using ebyteLearner.DTOs.Question;
using ebyteLearner.Helpers;
using ebyteLearner.Models;

namespace ebyteLearner.Data.Repository
{
    public interface IQuestionRepository
    {
        Task<QuestionDTO> Create(CreateQuestionRequestDTO request);
        Task<QuestionDTO> Update(Guid id, UpdateQuestionRequestDTO request);
        Task<QuestionDTO> Read(Guid id);
        Task Delete(Guid id);
    }
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DBContextService _dbContext;
        private readonly ILogger<QuestionRepository> _logger;
        private readonly IMapper _mapper;
        public QuestionRepository(DBContextService dbContext, ILogger<QuestionRepository> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<QuestionDTO> Create(CreateQuestionRequestDTO request)
        {
            if (!_dbContext.Pdf.Any(x => x.Id.ToString() == request.PDFId.ToString()))
                throw new AppException("PDF '" + request.PDFId + "' not found or does not exist");
            
            if (_dbContext.Question.FirstOrDefault(p => p.PDFId == request.PDFId && p.Slide == request.Slide) != null)
                throw new AppException("PDF '" + request.PDFId + "' already has a question in slide " + request.Slide);
            
            var questionObj = new Question
            {
                Slide = request.Slide,
                QuestionName = request.QuestionName,
                Score = request.Score,
                PDFId = request.PDFId,
                CreatedDate = request.CreatedDate
            };

            var question = _mapper.Map<Question>(questionObj);

            _dbContext.Question.Add(question);

            await _dbContext.SaveChangesAsync();

            var questionResponse = _mapper.Map<QuestionDTO>(questionObj);
            return questionResponse;
        }

        public async Task<QuestionDTO> Update(Guid id, UpdateQuestionRequestDTO request)
        {
            var questionDB = await _dbContext.Question.FindAsync(id);
            if (questionDB != null)
            {
                _mapper.Map(request, questionDB);
                await _dbContext.SaveChangesAsync();
                var updatedQuestionResponse = _mapper.Map<QuestionDTO>(questionDB);
                return updatedQuestionResponse;
            }
            else
            {
                throw new AppException("Question '" + id + "' not found");
            }
        }

        public async Task<QuestionDTO> Read(Guid id)
        {
            var questionDB = await _dbContext.Question.FindAsync(id);
            if (questionDB != null)
            {
                var questionResponse = _mapper.Map<QuestionDTO>(questionDB);
                return questionResponse;
            }
            else
                throw new AppException("Question '" + id + "' not found");
        }

        public async Task Delete(Guid id)
        {
            var questionDB = await _dbContext.Question.FindAsync(id);
            if (questionDB != null)
            {
                _dbContext.Remove(questionDB);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new AppException($"Question '{id}' not found");
            }
        }
    }
}
