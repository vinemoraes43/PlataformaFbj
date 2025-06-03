// Services/AuthService.cs
using Microsoft.EntityFrameworkCore;
using PlataformaFbj.Data;
using PlataformaFbj.Enums;
using PlataformaFbj.Models;
using PlataformaFbj.Dto.Auth.Requests;

namespace PlataformaFbj.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<string> Registrar(RegisterRequestDto dto)
        {
            if (!Enum.TryParse(dto.Tipo, out TipoUsuario tipoUsuario))
            {
                throw new ArgumentException("Tipo de usuário inválido");
            }

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Tipo = tipoUsuario
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return _tokenService.GenerateToken(usuario);
        }

        public async Task<Usuario> Authenticate(string email, string senha)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash))
            {
                throw new Exception("Email ou senha incorretos");
            }

            return usuario;
        }

        public async Task<string> Login(LoginRequestDto dto)
        {
            var usuario = await Authenticate(dto.Email, dto.Senha);
            await AplicarDelaySeNecessario(dto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            {
                if (usuario != null)
                {
                    usuario.TentativasLogin++;
                    usuario.UltimaTentativa = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                throw new Exception("Credenciais inválidas");
            }

            usuario.TentativasLogin = 0;
            await _context.SaveChangesAsync();

            return _tokenService.GenerateToken(usuario);
        }

        public async Task AplicarDelaySeNecessario(string email)
        {
            var usuario = await _context.Usuarios
                        .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario?.TentativasLogin >= 3)
            {
                int delaySegundos = Math.Min(usuario.TentativasLogin * 2, 10);
                await Task.Delay(delaySegundos * 1000);
            }
        }
    }
}