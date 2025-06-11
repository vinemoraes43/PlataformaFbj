namespace PlataformaFbj.Dto.Feedbacks.Responses
{
    public class FeedbackResponseDto
    {
        public int Id { get; set; }
        public string Comentario { get; set; }
        public int Avaliacao { get; set; }
        public string Usuario { get; set; } // Nome do usuário que fez o feedback
        public DateTime DataCriacao { get; set; }
    }
}