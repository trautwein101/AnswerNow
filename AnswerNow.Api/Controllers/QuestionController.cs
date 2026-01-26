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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAll()
        {

            var result = await _questionService.GetAllDtosAsync();

            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<QuestionDto>> GetById(int id)
        {
            var result = await _questionService.GetByIdDtoAsync(id);
            return result == null ? NotFound() : Ok(result);

        }


        [HttpPost]
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

            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto); // 201 created

        }


    }
}
