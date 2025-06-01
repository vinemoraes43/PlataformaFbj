using Microsoft.AspNetCore.Mvc;
using PlataformaFbj.Service;
using PlataformaFbj.Dto.Auth.Requests;
using PlataformaFbj.Dto.Auth.Responses;
using AutoMapper;
using PlataformaFbj.Models;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;

    public AuthController(AuthService authService, IMapper mapper, ILogger<AuthController> logger)
    {
        _authService = authService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegisterRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Tentativa de registro com dados inválidos");
                return BadRequest(new ErrorResponses
                {
                    Success = false,
                    Message = "Dados inválidos",
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            var usuario = _mapper.Map<Usuario>(dto);
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            var token = await _authService.Registrar(dto);
            _logger.LogInformation($"Novo usuário registrado: {dto.Email}");

            var response = _mapper.Map<RegistrationResponse>(usuario);
            response.Token = token;

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no registro");
            return BadRequest(new ErrorResponses { Message = ex.Message });
        }
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        try
        {
            var usuario = await _authService.Authenticate(dto.Email, dto.Senha);
            var token = await _authService.Login(dto);
            _logger.LogInformation($"Login bem-sucedido: {dto.Email}");

            var response = _mapper.Map<LoginResponse>(usuario);
            response.Token = token;

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no login");
            return Unauthorized(new ErrorResponses { Message = ex.Message });
        }
    }
}