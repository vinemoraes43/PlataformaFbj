using System;
using PlataformaFbj.Enums;

namespace PlataformaFbj.Dto.Jogos.Responses
{
    public class JogoResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public PlataformaJogo Plataforma { get; set; }
        public string Genero { get; set; }
        public DateTime DataLancamento { get; set; }
        public int DesenvolvedorId { get; set; }
        public string Desenvolvedor { get; set; } // Nome do desenvolvedor
        public double MediaAvaliacoes { get; set; }
    }
}