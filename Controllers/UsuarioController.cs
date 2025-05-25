using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaFbj.Data;
using PlataformaFbj.Models;
using PlataformaFbj.Enums;
using BCrypt.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaFbj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios
                .Select(u => new Usuario
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Tipo = u.Tipo
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Select(u => new Usuario
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Tipo = u.Tipo
                })
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
}

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
                return BadRequest("Email já cadastrado");

            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, HideSensitiveData(usuario));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest();

            var existingUser = await _context.Usuarios.FindAsync(id);
            if (existingUser == null) return NotFound();

            if (usuario.Email != existingUser.Email && 
                await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
                return BadRequest("Email já cadastrado");

            existingUser.Nome = usuario.Nome;
            existingUser.Email = usuario.Email;
            existingUser.Tipo = usuario.Tipo;

            if (!string.IsNullOrWhiteSpace(usuario.SenhaHash))
                existingUser.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Feedbacks)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null) return NotFound();

            if (usuario.Tipo == TipoUsuario.Desenvolvedor && 
                await _context.Jogos.AnyAsync(j => j.DesenvolvedorId == id))
                return BadRequest("Desenvolvedor possui jogos cadastrados");

            _context.Feedbacks.RemoveRange(usuario.Feedbacks);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id) => 
            _context.Usuarios.Any(e => e.Id == id);

        private static Usuario HideSensitiveData(Usuario usuario) => 
            new()
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Tipo = usuario.Tipo
            };
    }
}