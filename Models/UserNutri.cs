namespace ProjetoIntegrador.Models
{
    public class UserNutri
    {
        public int Id { get; set; }
        public Usuarios Usuario{  get; set; }
        public Usuarios Nutricionista { get; set; }
        public bool Aceito { get; set; }
    }
}
