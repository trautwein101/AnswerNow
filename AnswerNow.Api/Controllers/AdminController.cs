using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnswerNow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;

        public AdminController(IAuthService authService, IAdminService adminService) 
        {
            _authService = authService;
            _adminService = adminService;
        }


        /// <summary>
        /// Retrieves admin dashboard statistics.
        /// </summary>
        /// <response code="200">Returns admin statistics.</response>
        /// <response code="404">If statistics are not available.</response>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(AdminStatsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AdminStatsDto>> GetStatsAsync()
        {
            var stats = await _adminService.GetAdminStatsAsync();

            return stats == null ? NotFound() : Ok(stats.ToDto()); 
        }


        /// <summary>
        /// Retrieves all users (admin view).
        /// </summary>
        /// <response code="200">Returns a list of users.</response>
        /// <response code="404">If no users are found.</response>
        [HttpGet("users")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = await _adminService.GetAllUsersAsync();

            if (users == null)
            {
                return NotFound();
            }

            var result = users.Select(a => a.ToDto());

            return Ok(result);
        }


        /// <summary>
        /// Changes a user's role.
        /// </summary>
        /// <param name="id">The unique ID of the user.</param>
        /// <param name="newRole">The new role value (e.g., User, Moderator, Admin).</param>
        /// <response code="200">Returns the updated user.</response>
        /// <response code="400">If the role value is invalid.</response>
        [HttpPost("{id:int}/role")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> ChangeUserRoleAsync(int id, [FromQuery] string newRole)
        {
            var role = await _adminService.ChangeUserRoleAsync(id, newRole);

            return role == null ? BadRequest("Invalid role") : Ok(role.ToDto());

        }

        /// <summary>
        /// Sets a user's active status.
        /// </summary>
        /// <param name="id">The unique ID of the user.</param>
        /// <param name="isActive">True to activate; false to deactivate.</param>
        /// <response code="200">Returns the updated user.</response>
        /// <response code="404">If the user does not exist.</response>
        [HttpPost("{id:int}/activated")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> SetUserActivateStatusAsync(int id, [FromQuery] bool isActive)
        {
            var user = await _adminService.SetUserActiveStatusAsync(id, isActive);

            return user == null ? NotFound() : Ok(user.ToDto());

        }

        /// <summary>
        /// Sets a user's inactive status.
        /// </summary>
        /// <param name="id">The unique ID of the user.</param>
        /// <param name="isInActive">True to mark inactive; false otherwise.</param>
        /// <response code="200">Returns the updated user.</response>
        /// <response code="404">If the user does not exist.</response>
        [HttpPost("{id:int}/inactivated")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> SetUserInActivateStatusAsync(int id, [FromQuery] bool isInActive)
        {
            var user = await _adminService.SetUserInActiveStatusAsync(id, isInActive);

            return user == null ? NotFound() : Ok(user.ToDto());

        }

        /// <summary>
        /// Sets a user's pending status.
        /// </summary>
        /// <param name="id">The unique ID of the user.</param>
        /// <param name="isPending">True to mark pending; false otherwise.</param>
        /// <response code="200">Returns the updated user.</response>
        /// <response code="404">If the user does not exist.</response>
        [HttpPost("{id:int}/pending")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> SetUserPendingStatusAsync(int id, [FromQuery] bool isPending)
        {
            var user = await _adminService.SetUserPendingStatusAsync(id, isPending);

            return user == null ? NotFound() : Ok(user.ToDto());

        }

        /// <summary>
        /// Sets a user's suspended status.
        /// </summary>
        /// <param name="id">The unique ID of the user.</param>
        /// <param name="isSuspended">True to suspend; false otherwise.</param>
        /// <response code="200">Returns the updated user.</response>
        /// <response code="404">If the user does not exist.</response>
        [HttpPost("{id:int}/suspended")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> SetUserSuspendStatusAsync(int id, [FromQuery] bool isSuspended)
        {
            var user = await _adminService.SetUserSuspendStatusAsync(id, isSuspended);

            return user == null ? NotFound() : Ok(user.ToDto());

        }

        /// <summary>
        /// Sets a user's banned status.
        /// </summary>
        /// <param name="id">The unique ID of the user.</param>
        /// <param name="isBanned">True to ban; false to unban.</param>
        /// <response code="200">Returns the updated user.</response>
        /// <response code="404">If the user does not exist.</response>
        [HttpPost("{id:int}/banned")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> SetUserBanStatusAsync(int id, [FromQuery] bool isBanned)
        {
            var user = await _adminService.SetUserBanStatusAsync(id, isBanned);

            return user == null ? NotFound() : Ok(user.ToDto());

        }

    }
}
