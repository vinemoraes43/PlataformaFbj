// Models/Usuario.cs
using System.Collections.Generic;

namespace PlataformaFbj.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        // Propriedade de navegação para os feedbacks do usuário
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}