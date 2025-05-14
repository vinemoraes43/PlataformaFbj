namespace PlataformaFbj.Models 
{
    // Models/Feedback.cs
public class Feedback
{
    public int Id { get; set; }
    public string Comentario { get; set; }
    public int Avaliacao { get; set; } // Ex: 1 a 5 estrelas
    public DateTime DataPublicacao { get; set; }

    // Relação: Um feedback pertence a um usuário e a um jogo
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public int JogoId { get; set; }
    public Jogo Jogo { get; set; }
}
}