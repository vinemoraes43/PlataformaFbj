using System.ComponentModel.DataAnnotations;

namespace PlataformaFbj.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        // Relacionamentos (obrigatórios)
        [Required]
        public int JogoId { get; set; }
        public Jogo Jogo { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // Dados do feedback
        [Required]
        [Range(1, 5, ErrorMessage = "A nota deve ser entre 1 e 5 estrelas.")]
        public int Nota { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "O comentário não pode ultrapassar 500 caracteres.")]
        public string Comentario { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataPublicacao { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;
    }
}