using Microsoft.EntityFrameworkCore;
using PlataformaFbj.Data;
using PlataformaFbj.Dto;
using PlataformaFbj.Enums;
using PlataformaFbj.Models;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public AuthService(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<string> Registrar(RegisterDto dto)
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

    public async Task<string> Login(LoginDto dto)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            throw new Exception("Credenciais inválidas");

        return _tokenService.GenerateToken(usuario);
    }
}