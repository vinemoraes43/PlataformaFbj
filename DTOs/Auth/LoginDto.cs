// DTOs/Auth/LoginDto.cs
namespace PlataformaFbj.Dto
{
    using System.ComponentModel.DataAnnotations;

    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(20)]
        public string Senha { get; set; }
    }
}