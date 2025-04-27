namespace ProjetoIntegrador.Models
{
    public class Layout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<AlimentoLayout> Cafe { get; set; }
        public ICollection<AlimentoLayout> Almoco { get; set; }
        public ICollection<AlimentoLayout> CafeDT { get; set; }
        public ICollection<AlimentoLayout> Janta { get; set; }
        public Usuarios Nutri { get; set; }
    }
}
