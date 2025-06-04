using PlataformaFbj.Enums;

namespace PlataformaFbj.Dto.Usuarios.Responses
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public TipoUsuario Tipo { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}