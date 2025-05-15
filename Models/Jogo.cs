using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PlataformaFbj.Enums;

namespace PlataformaFbj.Models
{
    public class Jogo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do jogo é obrigatório.")]
        public string Nome { get; set; }

        public string Descricao { get; set; }

        [Required]
        [EnumDataType(typeof(PlataformaJogo))]
        public PlataformaJogo Plataforma { get; set; }

        public string Genero { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataLancamento { get; set; }

        // Relação com Desenvolvedor (1:N)
        public int DesenvolvedorId { get; set; }
        public Usuario Desenvolvedor { get; set; }

        // Relação com Feedback (1:N)
        public ICollection<Feedback> Feedbacks { get; set; } = new HashSet<Feedback>();
    }
}