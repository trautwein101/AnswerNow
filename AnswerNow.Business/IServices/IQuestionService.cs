using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{

    /// <summary>
    /// Provides business logic for managing questions.
    /// </summary>
    public interface IQuestionService
    {
        /// <summary>
        /// Retrieves all questions.
        /// </summary>
        Task<IEnumerable<Question>> GetAllAsync();

        /// <summary>
        /// Retrieves all questions as DTOs including related user information.
        /// </summary>
        Task<IEnumerable<QuestionDto>> GetAllDtosAsync();

        /// <summary>
        /// Retrieves a question by its unique identifier.
        /// </summary>
        /// <param name="id">The unique ID of the question.</param>
        Task<Question?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves a question as a DTO by its unique identifier.
        /// </summary>
        /// <param name="id">The unique ID of the question.</param>
        Task<QuestionDto?> GetByIdDtoAsync(int id);

        /// <summary>
        /// Creates a new question.
        /// </summary>
        /// <param name="question">The question to create.</param>
        Task<Question> CreateAsync(Question question);

        /// <summary>
        /// Flags or unflags a question depending on user role and current state.
        /// </summary>
        /// <param name="questionId">The unique ID of the question.</param>
        /// <param name="isFlagged">
        /// True to flag the question; false to unflag it (moderators/admins only).
        /// </param>
        /// <returns>
        /// The updated question if successful, or null if the question does not exist.
        /// </returns>
        Task<Question?> FlaggedAsync(int questionId, bool isFlagged);

    }
}
