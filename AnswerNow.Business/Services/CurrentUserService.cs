using AnswerNow.Business.IServices;
using AnswerNow.Domain.Enums;
using AnswerNow.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AnswerNow.Business.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <inheritdoc />
        public CurrentUser Get()
        {
            var user = _contextAccessor.HttpContext?.User;

            var isAuth = user?.Identity?.IsAuthenticated == true;

            int? userId = null;

            var idVal = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(int.TryParse(idVal, out var id))
            {
                userId = id;
            }

            UserRole? role = null;
            var roleVal = user?.FindFirst(ClaimTypes.Role)?.Value;
            if (Enum.TryParse<UserRole>(roleVal, out var parsedRole))
            {
                role = parsedRole;
            }

            return new CurrentUser
            {
                IsAuthenticated = isAuth,
                UserId = userId,
                Role = role
            };

        }


    }
}
