using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Mappings;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AnswerNow.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {

        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }


        /// <summary>
        /// Retrieves all answers.
        /// </summary>
        /// <response code="200">Returns a list of answers.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDto>>> GetAll()
        {
            var result = await _answerService.GetAllDtosAsync();

            return Ok(result);
        }


        /// <summary>
        /// Retrieves an answer by ID.
        /// </summary>
        /// <param name="id">The unique ID of the answer.</param>
        /// <response code="200">Returns the answer.</response>
        /// <response code="404">If the answer does not exist.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AnswerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AnswerDto>> GetById(int id)
        {
            var result = await _answerService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);

        }


        /// <summary>
        /// Retrieves all answers for a specific question.
        /// </summary>
        /// <param name="questionId">The unique ID of the question.</param>
        /// <response code="200">Returns a list of answers for the question.</response>
        [HttpGet("question/{questionId:int}/answers")]
        [ProducesResponseType(typeof(IEnumerable<AnswerDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AnswerDto>>> GetByQuestionId(int questionId)
        {
            var answers = await _answerService.GetByQuestionIdAsync(questionId);

            var result = answers.Select(a => a.ToDto());

            return Ok(result);

        }


        /// <summary>
        /// Creates a new answer for a question.
        /// </summary>
        /// <param name="questionId">The unique ID of the question this answer belongs to.</param>
        /// <param name="dto">The answer payload.</param>
        /// <response code="201">Returns the newly created answer.</response>
        /// <response code="400">If the payload is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(AnswerDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AnswerDto>> Create(int questionId, [FromBody] AnswerDto dto)
        {

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            dto.QuestionId = questionId;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.UserId = userId;
                }
            }

            var created = await _answerService.CreateAsync(dto.ToEntity());

            var createdDto = created.ToDto();

            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);

        }


        /// <summary>
        /// Casts an upvote or downvote on an answer.
        /// </summary>
        /// <param name="id">The unique ID of the answer.</param>
        /// <param name="isUpVote">True to upvote; false to downvote.</param>
        /// <response code="200">Returns the updated answer.</response>
        /// <response code="404">If the answer does not exist.</response>
        [HttpPost("{id:int}/vote")]
        [ProducesResponseType(typeof(AnswerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AnswerDto>> Vote(int id, [FromQuery] bool isUpVote)
        {
            var answer = await _answerService.VoteAsync(id, isUpVote);

            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer.ToDto());
        }


        /// <summary>
        /// Flags or unflags an answer.
        /// </summary>
        /// <param name="id">The unique ID of the answer.</param>
        /// <param name="isFlagged">True to flag; false to unflag (moderators/admins only).</param>
        /// <response code="200">Returns the updated answer.</response>
        /// <response code="403">If the user is not allowed to perform this action.</response>
        /// <response code="404">If the answer does not exist.</response>
        [HttpPost("{id:int}/flagged")]
        [ProducesResponseType(typeof(AnswerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AnswerDto>> Flagged(int id, [FromQuery] bool isFlagged)
        {
            var answer = await _answerService.FlaggedAsync(id, isFlagged);

            if(answer == null)
            {
                return NotFound();
            }

            return Ok(answer.ToDto());
        }



    }
}
