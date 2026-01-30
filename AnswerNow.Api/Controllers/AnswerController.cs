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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDto>>> GetAll()
        {
            var result = await _answerService.GetAllDtosAsync();

            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<AnswerDto>> GetById(int id)
        {
            var result = await _answerService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);

        }


        // GET /api/Answer/question/42/answers
        [HttpGet("question/{questionId:int}/answers")]
        public async Task<ActionResult<IEnumerable<AnswerDto>>> GetByQuestionId(int questionId)
        {
            var answers = await _answerService.GetByQuestionIdAsync(questionId);

            var result = answers.Select(a => a.ToDto());

            return Ok(result);

        }


        [HttpPost]
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

            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto); // 201 created

        }


        // POST /api/Answer/5/vote?isUpVote=true
        [HttpPost("{id:int}/vote")]
        public async Task<ActionResult<AnswerDto>> Vote(int id, [FromQuery] bool isUpVote)
        {
            var answer = await _answerService.VoteAsync(id, isUpVote);

            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer.ToDto());
        }


        // POST /api/Answer/5/flagged?isFlagged=true
        [HttpPost("{id:int}/flagged")]
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
