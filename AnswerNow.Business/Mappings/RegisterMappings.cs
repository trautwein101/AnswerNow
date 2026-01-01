using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Mappings
{
    public static class RegisterMappings
    {

        public static RegisterDto ToDto(this Register entity)
        {
            return new RegisterDto
            {
                Email = entity.Email,
                DisplayName = entity.DisplayName,
                Password = entity.Password
            };
        }

        public static Register ToEntity(this RegisterDto dto)
        {
            return new Register
            {
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                Password = dto.Password
            };
        }


    }
}
