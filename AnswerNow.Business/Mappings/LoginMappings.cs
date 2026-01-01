
using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Mappings
{
    public static class LoginMappings
    {

        public static LoginDto ToDto(this Login dto)
        {
            return new LoginDto
            {
                Email = dto.Email,
                Password = dto.Password
            };
        }

        public static Login ToEntity(this LoginDto entity)
        {
            return new Login
            {
                Email = entity.Email,
                Password = entity.Password,
            };
        }


    }
}
