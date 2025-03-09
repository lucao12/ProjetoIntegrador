using Microsoft.EntityFrameworkCore;

namespace ProjetoIntegrador.Models
{
    public class Mensagem
    {
        public int Id { get; set; }
        public Usuarios Usuario { get; set; }
        public Usuarios Nutricionista { get; set; }
        public string Mensage { get; set; }

    }
}
