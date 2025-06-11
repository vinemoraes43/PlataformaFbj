using PlataformaFbj.Dto.Feedbacks.Responses.Shared;

namespace PlataformaFbj.Dto.Feedbacks.Responses
{
    public class FeedbackResponseDto
    {
        public int Id { get; set; }
        public string Comentario { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public int Nota { get; set; }
        public JogoSimpleDto Jogo { get; set; }
        public UsuarioSimpleDto Usuario { get; set; }
    }
}