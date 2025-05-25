// DTOs/Auth/RegisterDto.cs
using System.ComponentModel.DataAnnotations;
using PlataformaFbj.Enums;

namespace PlataformaFbj.Dto
{
    public class RegisterDto : LoginDto
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        [EnumDataType(typeof(TipoUsuario))]
        public string Tipo { get; set; } // "Desenvolvedor" ou "BetaTester"
    }
}