using PlataformaFbj.Dto.Auth.Requests;
using PlataformaFbj.Models;

namespace PlataformaFbj.Service
{
    public interface IAuthService
    {
        Task<string> Registrar(RegisterRequestDto dto);
        Task<Usuario> Authenticate(string email, string senha);
        Task<string> Login(LoginRequestDto dto);
        Task AplicarDelaySeNecessario(string email);
    }
}