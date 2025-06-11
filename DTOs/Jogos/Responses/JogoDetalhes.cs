using System;
using System.Collections.Generic;
using PlataformaFbj.Dto.Feedbacks.Responses;
using PlataformaFbj.Enums;

namespace PlataformaFbj.Dto.Jogos.Responses
{
    public class JogoDetalhesDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public PlataformaJogo Plataforma { get; set; }
        public string Genero { get; set; }
        public DateTime DataLancamento { get; set; }
        public int DesenvolvedorId { get; set; }
        public string Desenvolvedor { get; set; } // Nome do desenvolvedor
        public List<FeedbackResponseDto> Feedbacks { get; set; }
    }
}