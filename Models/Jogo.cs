namespace PlataformaFbj.Models
{
public class Jogo
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Genero { get; set; }
    public string Plataforma { get; set; } // Ex: PC, PlayStation, Xbox
    public DateTime DataLancamento { get; set; }

    // Relação: Um jogo pode ter vários feedbacks
    public ICollection<Feedback> Feedbacks { get; set; }
}
}