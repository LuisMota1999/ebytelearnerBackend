using AutoMapper;
using ebyteLearner.DTOs.Question;
using ebyteLearner.Helpers;
using ebyteLearner.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ebyteLearner.Data.Repository
{
    public interface IQuestionRepository
    {
        Task<int> Create(CreateQuestionRequestDTO request);
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

        public async Task<int> Create(CreateQuestionRequestDTO request)
        {
            if (!_dbContext.Pdf.Any(x => x.Id.ToString() == request.PDFId.ToString()))
                throw new ValidationException("PDF '" + request.PDFId + "' not found or does not exist");

            if (_dbContext.Question.FirstOrDefault(p => p.PDFId == request.PDFId && p.QuestionSlide == request.QuestionSlide) != null)
                throw new ValidationException("PDF '" + request.PDFId + "' already has a question in slide " + request.QuestionSlide);

            if (request.QuestionAnswers.Count < 4)
                throw new ValidationException("Required atleast 4 answers per question");

            var question = _mapper.Map<Question>(request);

            _dbContext.Question.Add(question);

            try
            {
                var rowsAffected = await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Created question with ID: {question.Id}, rows affected: {rowsAffected}");

                return rowsAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating question");

                throw;
            }
        }

        public async Task<QuestionDTO> Update(Guid id, UpdateQuestionRequestDTO request)
        {
            // Parameter validation
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID.", nameof(id));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Update request cannot be null.");
            }

            // Retrieve the question entity from the database
            var questionDB = await _dbContext.Question.FindAsync(id);
            if (questionDB == null)
            {
                throw new AppException($"Question with ID '{id}' not found.");
            }

            try
            {
                // Map the properties from the request object to the retrieved question entity
                _mapper.Map(request, questionDB);

                // Set the state of the entity to Modified
                _dbContext.Entry(questionDB).State = EntityState.Modified;

                // Save changes to the database within a transaction
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return the mapped DTO representing the updated question
                return _mapper.Map<QuestionDTO>(questionDB);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the exception and return an appropriate response
                _logger.LogError(ex, "Concurrency conflict occurred while updating the question.");
                throw new AppException("Concurrency conflict occurred while updating the question.", ex);
            }
            catch (DbUpdateException ex)
            {
                // Log the exception and return an appropriate response
                _logger.LogError(ex, "Error occurred while updating the question in the database.");
                throw new AppException("Error occurred while updating the question in the database.", ex);
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred while updating the question.");
                throw;
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
                _dbContext.Entry(questionDB).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new AppException($"Question '{id}' not found");
            }
        }
    }
}
