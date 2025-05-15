using Microsoft.EntityFrameworkCore;
using PlataformaFbj.Models;

namespace PlataformaFbj.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feedback>(entity =>
            {
                // Relação com Usuário
                entity.HasOne(f => f.Usuario)
                    .WithMany(u => u.Feedbacks)
                    .HasForeignKey(f => f.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict); // Impede cascade delete se necessário

                // Relação com Jogo
                entity.HasOne(f => f.Jogo)
                    .WithMany(j => j.Feedbacks)
                    .HasForeignKey(f => f.JogoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurações adicionais (opcional)
            modelBuilder.Entity<Jogo>(entity =>
            {
                entity.Property(j => j.Nome)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);
            });
        }
    }
}