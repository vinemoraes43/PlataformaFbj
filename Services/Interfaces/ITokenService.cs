using PlataformaFbj.Models;

namespace PlataformaFbj.Service
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario);
    }
}