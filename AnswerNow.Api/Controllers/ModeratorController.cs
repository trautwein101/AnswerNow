using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Mappings;
using AnswerNow.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnswerNow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles="Moderator, Admin")]
    public class ModeratorController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IModeratorService  _moderatorService;

        public ModeratorController(IAuthService authService, IModeratorService moderatorService)
        {
            _authService = authService;
            _moderatorService = moderatorService;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<ModeratorStatsDto>> GetStatsAsync()
        {
            var stats = await _moderatorService.GetModeratorStatsAsync();

            if(stats == null)
            {
                return NotFound(); //404 not found
            }

            return Ok(stats.ToDto()); 
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = await _moderatorService.GetAllUsersAsync();

            if (users == null)
                return NotFound();

            var result = users.Select(a => a.ToDto());

            return Ok(result);
        }


        //POST /api/Moderator/5/suspended?isSuspended=true
        [HttpPost("{id:int}/suspended")]
        public async Task<ActionResult<UserDto?>> setUserSuspendStatusAsync(int id, [FromQuery] bool isSuspended)
        {
            var suspended = await _moderatorService.SetUserSuspendStatusAsync(id, isSuspended);

            if(suspended == null)
            {
                return NotFound();
            }

            return Ok(suspended.ToDto());
        }


    }
}
