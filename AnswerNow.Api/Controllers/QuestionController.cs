using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Mappings;
using Microsoft.AspNetCore.Mvc;

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
            var questions = await _questionService.GetAllAsync();

            var result = questions.Select(q => q.ToDto());

            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<QuestionDto>> GetById(int id)
        {
            var question = await _questionService.GetByIdAsync(id);

            if (question == null)
            {
                return NotFound(); //404
            }

            var result = question.ToDto();

            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Create([FromBody] QuestionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var entity = dto.ToEntity();

            var created = await _questionService.CreateAsync(entity);

            var createdDto = created.ToDto();

            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);

        }


    }
}
