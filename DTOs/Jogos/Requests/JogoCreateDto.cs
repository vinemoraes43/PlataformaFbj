using System;
using System.ComponentModel.DataAnnotations;
using PlataformaFbj.Enums;

namespace PlataformaFbj.Dto.Jogos.Requests
{
    public class JogoCreateDto
    {
        [Required(ErrorMessage = "O nome do jogo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; }

        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A plataforma é obrigatória.")]
        [EnumDataType(typeof(PlataformaJogo), ErrorMessage = "Plataforma inválida.")]
        public PlataformaJogo Plataforma { get; set; }

        [StringLength(50, ErrorMessage = "O gênero não pode exceder 50 caracteres.")]
        public string Genero { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Formato de data inválido.")]
        public DateTime DataLancamento { get; set; }

        [Required(ErrorMessage = "O ID do desenvolvedor é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do desenvolvedor inválido.")]
        public int DesenvolvedorId { get; set; }
    }
}