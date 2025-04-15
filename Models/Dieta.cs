using Microsoft.EntityFrameworkCore;

namespace ProjetoIntegrador.Models
{
    public class Dieta
    {
        public int Id { get; set; }
        public string Dia { get; set; } // <-- Adicionado aqui
        public ICollection<AlimentoQuantidade> Cafe { get; set; }
        public ICollection<AlimentoQuantidade> Almoco { get; set; }
        public ICollection<AlimentoQuantidade> CafeDT { get; set; }
        public ICollection<AlimentoQuantidade> Janta { get; set; }
    }
}
