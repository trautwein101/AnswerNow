using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Mappings;
using AnswerNow.Business.Services;
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

        /// <summary>
        /// Retrieves moderator dashboard statistics.
        /// </summary>
        /// <response code="200">Returns moderator statistics.</response>
        /// <response code="404">If statistics are not available.</response>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(ModeratorStatsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ModeratorStatsDto>> GetStatsAsync()
        {
            var stats = await _moderatorService.GetModeratorStatsAsync();

            if(stats == null)
            {
                return NotFound();
            }

            return Ok(stats.ToDto()); 
        }

        /// <summary>
        /// Retrieves all users (moderator view).
        /// </summary>
        /// <response code="200">Returns a list of users.</response>
        /// <response code="404">If no users are found.</response>
        [HttpGet("users")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = await _moderatorService.GetAllUsersAsync();

            if (users == null)
                return NotFound();

            var result = users.Select(a => a.ToDto());

            return Ok(result);
        }


        /// <summary>
        /// Sets a user's suspended status (moderator action).
        /// </summary>
        /// <param name="id">The unique ID of the user.</param>
        /// <param name="isSuspended">True to suspend; false to unsuspend.</param>
        /// <response code="200">Returns the updated user.</response>
        /// <response code="404">If the user does not exist.</response>
        [HttpPost("{id:int}/suspended")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> SetUserSuspendStatusAsync(int id, [FromQuery] bool isSuspended)
        {
            var user = await _moderatorService.SetUserSuspendStatusAsync(id, isSuspended);

            return user == null ? NotFound() : Ok(user.ToDto());

        }



    }
}
