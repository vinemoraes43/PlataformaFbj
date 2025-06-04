using System.ComponentModel.DataAnnotations;
using PlataformaFbj.Enums;

namespace PlataformaFbj.Dto.Usuarios.Requests
{
    public class UsuarioUpdateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O tipo de usuário é obrigatório")]
        public TipoUsuario Tipo { get; set; }
    }
}