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

        [Required(ErrorMessage = "O campo Tipo é obrigatório")]
        [EnumDataType(typeof(TipoUsuario), 
            ErrorMessage = "Tipo de usuário inválido. Valores aceitos: Desenvolvedor, BetaTester")]
        public string Tipo { get; set; }

        // Validação customizada para senha forte
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
            ErrorMessage = "Use pelo menos: 1 letra maiúscula, 1 minúscula, 1 número, 1 caractere especial (@!?#) e mínimo 6 caracteres")]
        public new string Senha { get; set; }
    }
}