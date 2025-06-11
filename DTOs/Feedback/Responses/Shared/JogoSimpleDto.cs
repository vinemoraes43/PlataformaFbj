namespace PlataformaFbj.Dto.Feedbacks.Responses.Shared
{
    public class JogoSimpleDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string ImagemUrl { get; set; } // URL da capa do jogo
        public string Genero { get; set; }
        public double MediaAvaliacoes { get; set; }
    }
}