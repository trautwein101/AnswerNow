using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Mappings;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AnswerNow.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }


        /// <summary>
        /// Retrieves all questions.
        /// </summary>
        /// <response code="200">Returns a list of questions.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<QuestionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAll()
        {

            var result = await _questionService.GetAllDtosAsync();

            return Ok(result);
        }


        /// <summary>
        /// Retrieves a question by ID.
        /// </summary>
        /// <param name="id">The unique ID of the question.</param>
        /// <response code="200">Returns the question.</response>
        /// <response code="404">If the question does not exist.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<QuestionDto>> GetById(int id)
        {
            var result = await _questionService.GetByIdDtoAsync(id);
            return Ok(result);

        }


        /// <summary>
        /// Creates a new question.
        /// </summary>
        /// <param name="dto">The question payload.</param>
        /// <response code="201">Returns the newly created question.</response>
        /// <response code="400">If the payload is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<QuestionDto>> Create([FromBody] QuestionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            
            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    dto.UserId = userId;
                }
            }

            var created = await _questionService.CreateAsync(dto.ToEntity());

            var createdDto = created.ToDto();

            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);

        }


        /// <summary>
        /// Flags or unflags a question.
        /// </summary>
        /// <param name="id">The unique ID of the question.</param>
        /// <param name="isFlagged">True to flag; false to unflag (moderators/admins only).</param>
        /// <response code="200">Returns the updated question.</response>
        /// <response code="403">If the user is not allowed to perform this action.</response>
        /// <response code="404">If the question does not exist.</response>
        [HttpPost("{id:int}/flagged")]
        [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<QuestionDto>> Flagged(int id, [FromQuery] bool isFlagged)
        {
            var question = await _questionService.FlaggedAsync(id, isFlagged);

            if (question == null)
                return NotFound();

            return Ok(question.ToDto());
        }



    }
}
