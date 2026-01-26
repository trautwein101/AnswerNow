using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Mappings;
using AnswerNow.Data.IRepositories;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Services
{
    public class QuestionService : IQuestionService
    {

        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Question?> GetByIdAsync(int id)
        {
            return await _questionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _questionRepository.GetAllAsync();
        }

        public async Task<IEnumerable<QuestionDto>> GetAllDtosAsync()
        {
            var entities = await _questionRepository.GetAllWithUsersAsync();

           return entities.Select(q => q.ToDto());
        }


        public async Task<QuestionDto?> GetByIdDtoAsync(int id)
        {
            var entity = await _questionRepository.GetByIdWithUserAsync(id);
            return entity?.ToDto();
        }


        public async Task<Question> CreateAsync(Question question)
        {
          
            return await _questionRepository.CreateAsync(question);
        }


    }
}
