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

            if (stats == null)
            {
                return NotFound();
            }

            return Ok(stats.ToDto()); // 200 OK
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
        public async Task<ActionResult<UserDto?>> ChangeUserRoleAsync(int id, [FromQuery] string newRole)
        {
            var role = await _adminService.ChangeUserRoleAsync(id, newRole);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role.ToDto());
        }

        // POST /api/Admin/5/banned?isBanned=true
        [HttpPost("{id:int}/banned")]
        public async Task<ActionResult<UserDto?>> SetUserBanStatusAsync(int id, [FromQuery] bool isBanned)
        {
            var banned = await _adminService.SetUserBanStatusAsync(id, isBanned);

            if (banned == null)
            {
                return NotFound(); // 404
            }

            return Ok(banned.ToDto());

        }


    }
}
