using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaFbj.Data;
using PlataformaFbj.Models;
using PlataformaFbj.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using PlataformaFbj.Dto.Jogos.Responses;
using PlataformaFbj.Dto.Jogos.Requests;
using System.Security.Claims;
using PlataformaFbj.Dto;

[Route("api/v1/jogos")]
[ApiController]
[Authorize(Policy = "DesenvolvedorOuAdmin")]
public class JogoController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<JogoController> _logger;

    public JogoController(AppDbContext context, IMapper mapper, ILogger<JogoController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JogoResponseDto>>> GetJogos()
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var isAdmin = User.HasClaim(ClaimTypes.Role, "Admin");

            IQueryable<Jogo> query = _context.Jogos
                .Include(j => j.Desenvolvedor)
                .Include(j => j.Feedbacks);

            // Filtra por desenvolvedor se não for admin (dev so pode ver seus proprios jogos)
            if (!isAdmin)
            {
                query = query.Where(j => j.DesenvolvedorId == currentUserId);

                // Verifica se há jogos para o desenvolvedor
                if (!await query.AnyAsync())
                {
                    return Ok(new List<JogoResponseDto>()); // Retorna lista vazia se não houver jogos
                }
            }

            var jogos = await query.ToListAsync();
            return Ok(_mapper.Map<List<JogoResponseDto>>(jogos));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar jogos");
            return StatusCode(500, new ErrorResponse
            {
                Success = false,
                Message = "Erro interno ao listar jogos",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JogoDetalhesDto>> GetJogo(int id)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var isAdmin = User.HasClaim(ClaimTypes.Role, "Admin");

            var jogo = await _context.Jogos
                .Include(j => j.Desenvolvedor)
                .Include(j => j.Feedbacks)
                .ThenInclude(f => f.Usuario)
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jogo == null)
            {
                _logger.LogWarning("Jogo com ID {Id} não encontrado", id);
                return NotFound();
            }

            // Desenvolvedor só pode ver seus próprios jogos
            if (!isAdmin && jogo.DesenvolvedorId != currentUserId)
            {
                return Unauthorized(new ErrorResponse
                {
                    Success = false,
                    Message = "Você só pode visualizar os jogos que criou"
                });
            }

            return Ok(_mapper.Map<JogoDetalhesDto>(jogo));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar jogo ID {Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                Success = false,
                Message = "Erro interno ao buscar jogo",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<JogoResponseDto>> PostJogo([FromBody] JogoCreateDto jogoDto)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var isAdmin = User.HasClaim(ClaimTypes.Role, "Admin");

            // Desenvolvedor só pode criar jogos para si mesmo
            if (!isAdmin && currentUserId != jogoDto.DesenvolvedorId)
            {
                return Unauthorized(new ErrorResponse
                {
                    Success = false,
                    Message = "Você só pode criar jogos associados ao seu próprio usuário"
                });
            }

            if (!await ValidateDesenvolvedor(jogoDto.DesenvolvedorId))
                return BadRequest("Desenvolvedor inválido");

            var jogo = _mapper.Map<Jogo>(jogoDto);

            _context.Jogos.Add(jogo);
            await _context.SaveChangesAsync();

            await _context.Entry(jogo)
                .Reference(j => j.Desenvolvedor)
                .LoadAsync();

            var response = _mapper.Map<JogoResponseDto>(jogo);

            _logger.LogInformation("Jogo {Nome} criado pelo usuário {UserId}", jogo.Nome, currentUserId);

            return CreatedAtAction(nameof(GetJogo), new { id = jogo.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar jogo");
            return StatusCode(500, new ErrorResponse
            {
                Success = false,
                Message = "Erro interno ao criar jogo",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutJogo(int id, [FromBody] JogoUpdateDto jogoDto)
    {
        try
        {
            // Validação básica
            if (id != jogoDto.Id)
                return BadRequest("ID do jogo não corresponde");

            var jogoExistente = await _context.Jogos.FindAsync(id);
            if (jogoExistente == null)
                return NotFound();

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var isAdmin = User.IsInRole("Admin");

            // Validação de propriedade
            if (!isAdmin && jogoExistente.DesenvolvedorId != currentUserId)
                return Unauthorized(new ErrorResponse
                {
                    Success = false,
                    Message = "Você só pode editar seus próprios jogos"
                });

            // Validação de transferência
            if (jogoDto.DesenvolvedorId.HasValue)
            {
                if (!isAdmin)
                    return Unauthorized(new ErrorResponse
                    {
                        Success = false,
                        Message = "Apenas administradores podem transferir jogos"
                    });

                if (!await _context.Usuarios.AnyAsync(u => u.Id == jogoDto.DesenvolvedorId.Value && u.Tipo == TipoUsuario.Desenvolvedor))
                    return BadRequest(new ErrorResponse
                    {
                        Success = false,
                        Message = "Desenvolvedor não encontrado ou inválido"
                    });
            }

            // Atualização segura dos campos
            jogoExistente.Nome = jogoDto.Nome;
            jogoExistente.Descricao = jogoDto.Descricao;
            jogoExistente.Plataforma = (PlataformaJogo)jogoDto.Plataforma;
            jogoExistente.Genero = jogoDto.Genero;
            jogoExistente.DataLancamento = jogoDto.DataLancamento;

            // Só atualiza desenvolvedor se for admin e estiver especificado
            if (isAdmin && jogoDto.DesenvolvedorId.HasValue)
            {
                jogoExistente.DesenvolvedorId = jogoDto.DesenvolvedorId.Value;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro ao salvar no banco de dados");
            return StatusCode(500, new ErrorResponse
            {
                Success = false,
                Message = "Erro ao salvar alterações",
                Errors = new List<string> { ex.InnerException?.Message ?? ex.Message }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar jogo");
            return StatusCode(500, new ErrorResponse
            {
                Success = false,
                Message = "Erro interno ao atualizar jogo",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJogo(int id)
    {
        try
        {
            var jogo = await _context.Jogos
                .Include(j => j.Feedbacks)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jogo == null)
                return NotFound();

            _context.Feedbacks.RemoveRange(jogo.Feedbacks);
            _context.Jogos.Remove(jogo);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Jogo ID {Id} excluído por Admin", id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir jogo ID {Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                Success = false,
                Message = "Erro interno ao excluir jogo",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    private async Task<bool> ValidateDesenvolvedor(int desenvolvedorId)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Id == desenvolvedorId && u.Tipo == TipoUsuario.Desenvolvedor);
    }
}