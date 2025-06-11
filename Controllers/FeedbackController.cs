using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using PlataformaFbj.Data;
using PlataformaFbj.Models;
using PlataformaFbj.Dto.Feedbacks.Requests;
using PlataformaFbj.Dto.Feedbacks.Responses;
using PlataformaFbj.Dto.Feedbacks.Responses.Shared;
using PlataformaFbj.Dto;

namespace PlataformaFbj.Controllers
{
    [Route("api/v1/feedbacks")]
    [ApiController]
    [Authorize]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(
            AppDbContext context,
            IMapper mapper,
            ILogger<FeedbackController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// Lista todos os feedbacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackResponseDto>>> GetAll()
        {
            try
            {
                var feedbacks = await _context.Feedbacks
                    .Include(f => f.Jogo)
                    .Include(f => f.Usuario)
                    .OrderByDescending(f => f.DataPublicacao)
                    .AsNoTracking()
                    .ToListAsync();

                var response = _mapper.Map<List<FeedbackResponseDto>>(feedbacks);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar feedbacks");
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Erro interno ao listar feedbacks"
                });
            }
        }

        /// Obtém um feedback específico
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackResponseDto>> GetById(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks
                    .Include(f => f.Jogo)
                    .Include(f => f.Usuario)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (feedback == null)
                {
                    _logger.LogWarning("Feedback com ID {Id} não encontrado", id);
                    return NotFound(new ErrorResponse
                    {
                        Success = false,
                        Message = "Feedback não encontrado"
                    });
                }

                var response = _mapper.Map<FeedbackResponseDto>(feedback);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar feedback ID {Id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Erro interno ao buscar feedback"
                });
            }
        }

        /// Cria um novo feedback
        [HttpPost]
        public async Task<ActionResult<FeedbackResponseDto>> Create([FromBody] FeedbackCreateDto feedbackDto)
        {
            try
            {
                // Validação do modelo
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ErrorResponse
                    {
                        Success = false,
                        Message = "Dados inválidos",
                        Errors = errors
                    });
                }

                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Verifica se o usuário está tentando criar feedback para outro usuário
                if (currentUserId != feedbackDto.UsuarioId)
                {
                    return Unauthorized(new ErrorResponse
                    {
                        Success = false,
                        Message = "Você só pode criar feedbacks para si mesmo"
                    });
                }

                // Verifica se o jogo existe
                if (!await _context.Jogos.AnyAsync(j => j.Id == feedbackDto.JogoId))
                {
                    return BadRequest(new ErrorResponse
                    {
                        Success = false,
                        Message = "Jogo não encontrado"
                    });
                }

                // Verifica se o usuário já avaliou este jogo
                if (await _context.Feedbacks.AnyAsync(f =>
                    f.JogoId == feedbackDto.JogoId &&
                    f.UsuarioId == currentUserId))
                {
                    return BadRequest(new ErrorResponse
                    {
                        Success = false,
                        Message = "Você já avaliou este jogo"
                    });
                }

                var feedback = _mapper.Map<Feedback>(feedbackDto);
                feedback.DataPublicacao = DateTime.UtcNow;

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                // Carrega os relacionamentos para a resposta
                await _context.Entry(feedback)
                    .Reference(f => f.Jogo)
                    .LoadAsync();
                await _context.Entry(feedback)
                    .Reference(f => f.Usuario)
                    .LoadAsync();

                var response = _mapper.Map<FeedbackResponseDto>(feedback);

                _logger.LogInformation("Novo feedback criado - ID: {FeedbackId}, Jogo: {JogoId}, Usuário: {UsuarioId}",
                    feedback.Id, feedback.JogoId, feedback.UsuarioId);

                return CreatedAtAction(nameof(GetById), new { id = feedback.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar feedback");
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Erro interno ao criar feedback"
                });
            }
        }

        /// Atualiza um feedback existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FeedbackUpdateDto feedbackDto)
        {
            try
            {
                if (id != feedbackDto.Id)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Success = false,
                        Message = "ID do feedback não corresponde"
                    });
                }

                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Success = false,
                        Message = "Feedback não encontrado"
                    });
                }

                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Verifica se o usuário é o dono do feedback ou admin
                if (feedback.UsuarioId != currentUserId && !User.IsInRole("Admin"))
                {
                    return Unauthorized(new ErrorResponse
                    {
                        Success = false,
                        Message = "Você não tem permissão para editar este feedback"
                    });
                }

                // Atualiza apenas os campos permitidos
                feedback.Nota = feedbackDto.Nota;
                feedback.Comentario = feedbackDto.Comentario;
                feedback.DataAtualizacao = DateTime.UtcNow;

                _context.Feedbacks.Update(feedback);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Feedback atualizado - ID: {FeedbackId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar feedback ID {Id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Erro interno ao atualizar feedback"
                });
            }
        }

        /// Remove um feedback
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var feedback = await _context.Feedbacks.FindAsync(id);
                if (feedback == null)
                {
                    return NotFound(new ErrorResponse
                    {
                        Success = false,
                        Message = "Feedback não encontrado"
                    });
                }

                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Feedback removido - ID: {FeedbackId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover feedback ID {Id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Erro interno ao remover feedback"
                });
            }
        }

        /// Lista feedbacks por jogo
        [HttpGet("jogos/{jogoId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<FeedbackResponseDto>>> GetByJogo(int jogoId)
        {
            try
            {
                var feedbacks = await _context.Feedbacks
                    .Where(f => f.JogoId == jogoId)
                    .Include(f => f.Jogo)
                    .Include(f => f.Usuario)
                    .OrderByDescending(f => f.DataPublicacao)
                    .AsNoTracking()
                    .ToListAsync();

                var response = _mapper.Map<List<FeedbackResponseDto>>(feedbacks);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar feedbacks do jogo ID {JogoId}", jogoId);
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Erro interno ao buscar feedbacks"
                });
            }
        }

        /// Calcula a média de avaliações de um jogo
        [HttpGet("jogos/{jogoId}/media")]
        [AllowAnonymous]
        public async Task<ActionResult<double>> GetMediaAvaliacoes(int jogoId)
        {
            try
            {
                var media = await _context.Feedbacks
                    .Where(f => f.JogoId == jogoId)
                    .AverageAsync(f => (double?)f.Nota) ?? 0.0;

                return Ok(Math.Round(media, 1));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular média do jogo ID {JogoId}", jogoId);
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "Erro interno ao calcular média"
                });
            }
        }
    }
}