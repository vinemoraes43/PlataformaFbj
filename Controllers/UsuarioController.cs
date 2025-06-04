using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PlataformaFbj.Data;
using PlataformaFbj.Dto.Usuarios.Requests;
using PlataformaFbj.Dto.Usuarios.Responses;
using PlataformaFbj.Models;
using PlataformaFbj.Enums;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using PlataformaFbj.Dto;

namespace PlataformaFbj.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    [Authorize(Policy = "Admin")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(AppDbContext context, IMapper mapper, ILogger<UsuarioController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetUsuarios(
            [FromQuery] string nome,
            [FromQuery] TipoUsuario? tipo)
        {
            try
            {
                var query = _context.Usuarios.AsQueryable();

                // Aplica filtros
                if (!string.IsNullOrEmpty(nome))
                {
                    query = query.Where(u => u.Nome.Contains(nome));
                }

                if (tipo.HasValue)
                {
                    query = query.Where(u => u.Tipo == tipo.Value);
                }

                var usuarios = await query
                    .OrderBy(u => u.Nome)
                    .ToListAsync();

                var usuarioDtos = _mapper.Map<List<UsuarioResponseDto>>(usuarios);
                return Ok(usuarioDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar usuários");
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Erro interno ao listar usuários",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetUsuario(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    _logger.LogWarning("Usuário com ID {Id} não encontrado", id);
                    return NotFound(new ErrorResponse
                    {
                        Success = false,
                        Message = "Usuário não encontrado"
                    });
                }

                return Ok(_mapper.Map<UsuarioResponseDto>(usuario));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuário com ID {Id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Erro interno ao buscar usuário",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDto>> CreateUsuario(
            [FromBody] UsuarioCreateDto createDto)
        {
            try
            {
                // Verificar se email já existe
                if (await _context.Usuarios.AnyAsync(u => u.Email == createDto.Email))
                {
                    _logger.LogWarning("Tentativa de cadastrar email já existente: {Email}", createDto.Email);
                    return Conflict(new ErrorResponse
                    {
                        Success = false,
                        Message = "Email já cadastrado"
                    });
                }

                // Mapear DTO para entidade
                var usuario = _mapper.Map<Usuario>(createDto);

                // Campos adicionais
                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(createDto.Senha);
                usuario.DataCriacao = DateTime.UtcNow;

                // Salvar no banco
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Novo usuário criado por admin: {Email}", createDto.Email);

                // Retornar resposta
                return CreatedAtAction(
                    nameof(GetUsuario),
                    new { id = usuario.Id },
                    _mapper.Map<UsuarioResponseDto>(usuario));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar usuário");
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Erro interno ao criar usuário",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(
            int id,
            [FromBody] UsuarioUpdateDto updateDto)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    _logger.LogWarning("Tentativa de atualizar usuário inexistente: ID {Id}", id);
                    return NotFound(new ErrorResponse
                    {
                        Success = false,
                        Message = "Usuário não encontrado"
                    });
                }

                // Verificar conflito de email
                if (await _context.Usuarios
                    .AnyAsync(u => u.Email == updateDto.Email && u.Id != id))
                {
                    _logger.LogWarning("Conflito de email ao atualizar usuário ID {Id}", id);
                    return Conflict(new ErrorResponse
                    {
                        Success = false,
                        Message = "Email já está em uso por outro usuário"
                    });
                }

                // Mapear DTO para entidade existente
                _mapper.Map(updateDto, usuario);

                // Atualizar senha se fornecida
                if (!string.IsNullOrEmpty(updateDto.Senha))
                {
                    usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(updateDto.Senha);
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Usuário ID {Id} atualizado por admin", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário ID {Id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Erro interno ao atualizar usuário",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Feedbacks)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                {
                    _logger.LogWarning("Tentativa de excluir usuário inexistente: ID {Id}", id);
                    return NotFound(new ErrorResponse
                    {
                        Success = false,
                        Message = "Usuário não encontrado"
                    });
                }

                // Verificar se é desenvolvedor com jogos associados
                if (usuario.Tipo == TipoUsuario.Desenvolvedor &&
                    await _context.Jogos.AnyAsync(j => j.DesenvolvedorId == id))
                {
                    _logger.LogWarning(
                        "Tentativa de excluir desenvolvedor com jogos associados: ID {Id}", id);
                    return BadRequest(new ErrorResponse
                    {
                        Success = false,
                        Message = "Não é possível excluir - Desenvolvedor possui jogos cadastrados"
                    });
                }

                _context.Feedbacks.RemoveRange(usuario.Feedbacks);
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Usuário ID {Id} excluído por admin", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir usuário ID {Id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Erro interno ao excluir usuário",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}