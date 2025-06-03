using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PlataformaFbj.Service;
using PlataformaFbj.Dto.Auth.Requests;
using PlataformaFbj.Dto.Auth.Responses;
using PlataformaFbj.Models;
using PlataformaFbj.Filters;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper, ILogger<AuthController> logger)
    {
        _authService = authService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost("registrar")]
    [ValidateUniqueEmail]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegistrationResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> Registrar([FromBody] RegisterRequestDto dto)
    {
        _logger.LogInformation("Iniciando registro para {Email}", dto.Email);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Dados inválidos no registro para {Email}", dto.Email);
            return BadRequest(new ErrorResponse
            {
                Success = false,
                Message = "Dados inválidos",
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            });
        }

        try
        {
            var token = await _authService.Registrar(dto);
            _logger.LogInformation("Usuário {Email} registrado com sucesso", dto.Email);

            return Ok(new RegistrationResponse
            {
                Success = true,
                Token = token,
                Email = dto.Email,
                Message = "Registro realizado com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar usuário {Email}", dto.Email);
            return BadRequest(new ErrorResponse
            {
                Success = false,
                Message = "Erro ao registrar usuário",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        _logger.LogInformation("Tentativa de login para {Email}", dto.Email);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Dados inválidos no login para {Email}", dto.Email);
            return BadRequest(new ErrorResponse
            {
                Success = false,
                Message = "Dados inválidos",
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            });
        }

        try
        {
            var usuario = await _authService.Authenticate(dto.Email, dto.Senha);
            var token = await _authService.Login(dto);

            _logger.LogInformation("Login bem-sucedido para {Email}", dto.Email);

            return Ok(new LoginResponse
            {
                Success = true,
                Token = token,
                Email = usuario.Email,
                Nome = usuario.Nome,
                TipoUsuario = usuario.Tipo.ToString(),
                Message = "Login realizado com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o login para {Email}", dto.Email);
            return Unauthorized(new ErrorResponse
            {
                Success = false,
                Message = "Credenciais inválidas",
                Errors = new List<string> { ex.Message }
            });
        }
    }
}