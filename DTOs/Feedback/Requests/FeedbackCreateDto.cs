using System.ComponentModel.DataAnnotations;

namespace PlataformaFbj.Dto.Feedbacks.Requests
{
    public class FeedbackCreateDto
    {
        [Required(ErrorMessage = "O ID do jogo é obrigatório")]
        public int JogoId { get; set; }

        [Required(ErrorMessage = "O ID do usuario é obrigatório")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "A nota é obrigatória")]
        [Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5")]
        public int Nota { get; set; }

        [Required(ErrorMessage = "O comentário é obrigatório")]
        [StringLength(500, ErrorMessage = "O comentário não pode exceder 500 caracteres")]
        public string Comentario { get; set; }
    }
}