using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Mappings;
using AnswerNow.Data.IRepositories;
using AnswerNow.Domain.Enums;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Services
{
    public class QuestionService : IQuestionService
    {

        private readonly IQuestionRepository _questionRepository;
        private readonly ICurrentUserService _currentUserService;

        public QuestionService(IQuestionRepository questionRepository, ICurrentUserService currentUserService)
        {
            _questionRepository = questionRepository;
            _currentUserService = currentUserService;
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

        public async Task<Question?> FlaggedAsync(int questionId, bool isFlagged)
        {
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null) return null;

            var currentUser = _currentUserService.Get();

            // Admin/Moderator: full control
            if (currentUser.IsAtLeast(UserRole.Moderator))
            {
                if (question.IsFlagged)
                    return question; //not change

                question.IsFlagged = isFlagged;
                return await _questionRepository.UpdateAsync(question);
            }

            //Normal User: only allow false -> true
            if(!question.IsFlagged && isFlagged)
            {
                question.IsFlagged = true;
                return await _questionRepository.UpdateAsync(question);
            }

            //no change needed or allowed
            return question;

        }


    }
}
