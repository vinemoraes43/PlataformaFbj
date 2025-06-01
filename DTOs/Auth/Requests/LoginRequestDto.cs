// DTOs/Auth/Requests/LoginDto.cs
using System.ComponentModel.DataAnnotations;

namespace PlataformaFbj.Dto.Auth.Requests
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [MaxLength(100, ErrorMessage = "Email não pode exceder 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}