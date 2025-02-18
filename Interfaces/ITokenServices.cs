using ProjetoIntegrador.Models;

namespace ProjetoIntegrador.Interfaces
{
    public interface ITokenServices
    {
        string GenerateToken(Usuarios user);
    }
}
