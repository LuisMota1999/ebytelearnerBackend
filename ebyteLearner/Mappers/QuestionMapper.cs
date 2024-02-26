using AutoMapper;
using ebyteLearner.DTOs.Module;
using ebyteLearner.DTOs.Question;
using ebyteLearner.Models;

namespace ebyteLearner.Mappers
{
    public class QuestionMapper: Profile
    {
        public QuestionMapper()
        {
            CreateMap<CreateQuestionRequestDTO, QuestionDTO>();
            // Question -> QuestionDTO
            CreateMap<Question, QuestionDTO>();
            CreateMap<QuestionDTO, CreateQuestionRequestDTO>();
        }
            
    }
}
