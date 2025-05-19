using Microsoft.EntityFrameworkCore;
using PlataformaFbj.Models;
using PlataformaFbj.Enums;

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
            // Configuração da entidade Feedback
            modelBuilder.Entity<Feedback>(entity =>
            {
                // Relação com Usuário
                entity.HasOne(f => f.Usuario)
                    .WithMany(u => u.Feedbacks)
                    .HasForeignKey(f => f.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relação com Jogo
                entity.HasOne(f => f.Jogo)
                    .WithMany(j => j.Feedbacks)
                    .HasForeignKey(f => f.JogoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração da entidade Jogo
            modelBuilder.Entity<Jogo>(entity =>
            {
                entity.Property(j => j.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(j => j.Descricao)
                    .HasMaxLength(500);

                entity.Property(j => j.Genero)
                    .HasMaxLength(50);

                entity.Property(j => j.Plataforma)
                    .HasConversion<string>()
                    .IsRequired();

                // Relação com Desenvolvedor
                entity.HasOne(j => j.Desenvolvedor)
                    .WithMany()
                    .HasForeignKey(j => j.DesenvolvedorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração da entidade Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(u => u.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(u => u.SenhaHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.Role)
                    .HasConversion<string>()
                    .IsRequired();

                // Índice único para email
                entity.HasIndex(u => u.Email)
                    .IsUnique();
            });

            // Configurações adicionais de delete behavior
            modelBuilder.Entity<Jogo>()
                .HasMany(j => j.Feedbacks)
                .WithOne(f => f.Jogo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Feedbacks)
                .WithOne(f => f.Usuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}