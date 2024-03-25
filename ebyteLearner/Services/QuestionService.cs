using AutoMapper;
using ebyteLearner.Data.Repository;
using ebyteLearner.DTOs.Question;

namespace ebyteLearner.Services
{
    public interface IQuestionService
    {
        Task<QuestionDTO> GetQuestion(Guid id);
        Task<int> CreateQuestion(CreateQuestionRequestDTO request);
        Task<QuestionDTO> UpdateQuestion(Guid id, UpdateQuestionRequestDTO request);
        Task DeleteQuestion(Guid id);
    }

    public class QuestionService : IQuestionService
    {

        private readonly IQuestionRepository _questionRepository;
        private readonly IPDFRepository _pdfRepository;
        private readonly ILogger<QuestionService> _logger;
        private readonly IMapper _mapper;
        public QuestionService(IQuestionRepository questionRepository, IPDFRepository pdfRepository, ILogger<QuestionService> logger, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _pdfRepository = pdfRepository;
            _logger = logger;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<int> CreateQuestion(CreateQuestionRequestDTO request)
        {          
            return await _questionRepository.Create(request);
        }

        public async Task<QuestionDTO> GetQuestion(Guid id)
        {
            return await _questionRepository.Read(id);
        }

        public async Task<QuestionDTO> UpdateQuestion(Guid id, UpdateQuestionRequestDTO request)
        {
            return await _questionRepository.Update(id, request);
        }

        public async Task DeleteQuestion(Guid id)
        {
            await _questionRepository.Delete(id);
        }

        
    }
}
