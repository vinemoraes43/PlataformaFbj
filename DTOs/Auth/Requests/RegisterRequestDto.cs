// DTOs/Auth/Requests/RegisterDto.cs
using System.ComponentModel.DataAnnotations;
using PlataformaFbj.Enums;

namespace PlataformaFbj.Dto.Auth.Requests
{
    public class RegisterRequestDto : LoginRequestDto
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [StringLength(100, ErrorMessage = "O email não pode exceder {1} caracteres")]
        public new string Email { get; set; }

        // Validação customizada para senha forte
        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
            ErrorMessage = "Use pelo menos: 1 letra maiúscula, 1 minúscula, 1 número, 1 caractere especial (@!?#) e mínimo 6 caracteres")]
        public new string Senha { get; set; }

        [Required(ErrorMessage = "O campo Tipo é obrigatório")]
        [EnumDataType(typeof(TipoUsuario),
            ErrorMessage = "Tipo de usuário inválido. Valores aceitos: Desenvolvedor, BetaTester")]
        public string Tipo { get; set; }
    }
}