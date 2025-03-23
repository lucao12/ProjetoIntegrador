using Microsoft.EntityFrameworkCore;

namespace ProjetoIntegrador.Models
{
    public class DietaSemana
    {
        public int Id { get; set; }
        public Usuarios Usuario { get; set; }
        public Usuarios Nutricionista { get; set; }
        public ICollection<Dieta> Dietas { get; set; }
    }
}
