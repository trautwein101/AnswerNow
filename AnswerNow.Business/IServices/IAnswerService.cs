using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{

    /// <summary>
    /// Provides operations for managing answers.
    /// </summary>
    public interface IAnswerService
    {

        /// <summary>
        /// Retrieves all answers.
        /// </summary>
        Task<IEnumerable<Answer>> GetAllAsync();

        /// <summary>
        /// Retrieves all answers as DTOs.
        /// </summary>
        Task<IEnumerable<AnswerDto>> GetAllDtosAsync();

        /// <summary>
        /// Retrieves an answer by its unique identifier.
        /// </summary>
        /// <param name="id">The unique ID of the answer.</param>
        /// <returns>
        /// The <see cref="Answer"/> if found, or null otherwise.
        /// </returns>
        Task<Answer?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves all answers for a specific question.
        /// </summary>
        /// <param name="questionId">The unique ID of the question.</param>
        Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId);

        /// <summary>
        /// Creates a new answer.
        /// </summary>
        /// <param name="answer">The answer domain model.</param>
        /// <returns>
        /// The newly created <see cref="Answer"/>.
        /// </returns>
        Task<Answer> CreateAsync(Answer answer);

        /// <summary>
        /// Casts an upvote or downvote on an answer.
        /// </summary>
        /// <param name="answerId">The unique ID of the answer.</param>
        /// <param name="isUpVote">True to upvote; false to downvote.</param>
        /// <returns>
        /// The updated <see cref="Answer"/> if successful, or null if the answer does not exist.
        /// </returns>
        Task<Answer?> VoteAsync(int answerId, bool isUpVote);

        /// <summary>
        /// Flags or unflags an answer.
        /// </summary>
        /// <param name="answerId">The unique ID of the answer.</param>
        /// <param name="isFlagged">True to flag; false to unflag.</param>
        /// <returns>
        /// The updated <see cref="Answer"/> if successful, or null if the answer does not exist.
        /// </returns>
        Task<Answer?> FlaggedAsync(int answerId, bool isFlagged);

    }
}
