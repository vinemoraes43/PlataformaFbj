using System;
using System.ComponentModel.DataAnnotations;
using PlataformaFbj.Enums;

namespace PlataformaFbj.Dto.Jogos.Requests
{
    public class JogoUpdateDto
    {
        [Required(ErrorMessage = "O ID é obrigatório")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome não pode exceder 100 caracteres")]
        public string Nome { get; set; }

        [StringLength(500, ErrorMessage = "Descrição não pode exceder 500 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Plataforma é obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage = "Plataforma inválida")]
        public int Plataforma { get; set; }

        [Required(ErrorMessage = "Gênero é obrigatório")]
        public string Genero { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Data inválida")]
        public DateTime DataLancamento { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Desenvolvedor inválido")]
        public int? DesenvolvedorId { get; set; }
    }
}