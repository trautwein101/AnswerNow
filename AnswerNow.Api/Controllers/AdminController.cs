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

        [HttpGet("stats")]
        public async Task<ActionResult<AdminStatsDto>> GetStatsAsync()
        {
            var stats = await _adminService.GetAdminStatsAsync();

            return stats == null ? NotFound() : Ok(stats.ToDto()); // 404 or 200
        }

        [HttpGet("users")]
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

        // POST /api/Admin/5/role?newRole=User
        [HttpPost("{id:int}/role")]
        public async Task<ActionResult<UserDto>> ChangeUserRoleAsync(int id, [FromQuery] string newRole)
        {
            var role = await _adminService.ChangeUserRoleAsync(id, newRole);

            return role == null ? BadRequest("Invalid role") : Ok(role.ToDto()); // 400 or 200

        }

        //POST /api/Admin/{userId}/activated?isActive=true 
        [HttpPost("{id:int}/activated")]
        public async Task<ActionResult<UserDto>> SetUserActivateStatusAsync(int id, [FromQuery] bool isActive)
        {
            var user = await _adminService.SetUserActiveStatusAsync(id, isActive);

            return user == null ? NotFound() : Ok(user.ToDto()); // 404 or 200

        }

        //POST /api/Admin/{userId}/inactivated?isInActive=true 
        [HttpPost("{id:int}/inactivated")]
        public async Task<ActionResult<UserDto>> SetUserInActivateStatusAsync(int id, [FromQuery] bool isInActive)
        {
            var user = await _adminService.SetUserInActiveStatusAsync(id, isInActive);

            return user == null ? NotFound() : Ok(user.ToDto()); // 404 or 200

        }

        //POST /api/admin/5/pending?isPending=true
        [HttpPost("{id:int}/pending")]
        public async Task<ActionResult<UserDto>> SetUserPendingStatusAsync(int id, [FromQuery] bool isPending)
        {
            var user = await _adminService.SetUserPendingStatusAsync(id, isPending);

            return user == null ? NotFound() : Ok(user.ToDto()); // 404 or 200

        }

        //POST /api/Admin/5/suspended?isSuspended=true
        [HttpPost("{id:int}/suspended")]
        public async Task<ActionResult<UserDto>> SetUserSuspendStatusAsync(int id, [FromQuery] bool isSuspended)
        {
            var user = await _adminService.SetUserSuspendStatusAsync(id, isSuspended);

            return user == null ? NotFound() : Ok(user.ToDto()); // 404 or 200

        }

        // POST /api/Admin/5/banned?isBanned=true
        [HttpPost("{id:int}/banned")]
        public async Task<ActionResult<UserDto>> SetUserBanStatusAsync(int id, [FromQuery] bool isBanned)
        {
            var user = await _adminService.SetUserBanStatusAsync(id, isBanned);

            return user == null ? NotFound() : Ok(user.ToDto()); // 404 or 200

        }

    }
}
