using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaFbj.Data;
using PlataformaFbj.Models;
using PlataformaFbj.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaFbj.Controllers
{
    [Route("api/Jogos")]
    [ApiController]
    public class JogoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JogoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jogo>>> GetJogos()
        {
            return await _context.Jogos
                .Include(j => j.Desenvolvedor)
                .Include(j => j.Feedbacks)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Jogo>> GetJogo(int id)
        {
            var jogo = await _context.Jogos
                .Include(j => j.Desenvolvedor)
                .Include(j => j.Feedbacks)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jogo == null)
            {
            return NotFound();
            }
        return jogo;
        }

        [HttpPost]
        public async Task<ActionResult<Jogo>> PostJogo(Jogo jogo)
        {
            if (!await ValidateDesenvolvedor(jogo.DesenvolvedorId))
                return BadRequest("Desenvolvedor inválido");

            _context.Jogos.Add(jogo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJogo), new { id = jogo.Id }, jogo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutJogo(int id, Jogo jogo)
        {
            if (id != jogo.Id) return BadRequest();

            if (!await ValidateDesenvolvedor(jogo.DesenvolvedorId))
                return BadRequest("Desenvolvedor inválido");

            _context.Entry(jogo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JogoExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJogo(int id)
        {
            var jogo = await _context.Jogos
                .Include(j => j.Feedbacks)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jogo == null) return NotFound();

            _context.Feedbacks.RemoveRange(jogo.Feedbacks);
            _context.Jogos.Remove(jogo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JogoExists(int id) => 
            _context.Jogos.Any(e => e.Id == id);

        private async Task<bool> ValidateDesenvolvedor(int desenvolvedorId)
        {
            var desenvolvedor = await _context.Usuarios.FindAsync(desenvolvedorId);
            return desenvolvedor != null && desenvolvedor.Tipo == TipoUsuario.Desenvolvedor;
        }
    }
}