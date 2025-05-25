using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PlataformaFbj.Enums;

namespace PlataformaFbj.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string SenhaHash { get; set; }

        [Required]
        [EnumDataType(typeof(TipoUsuario))]
        public TipoUsuario Tipo { get; set; }

        // Relacionamento 1:N com Feedback (um usuário tem muitos feedbacks)
        public ICollection<Feedback> Feedbacks { get; set; } = new HashSet<Feedback>();
    }
}