namespace ProjetoIntegrador.Models
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Role { get; set; }
        public string Telefone { get; set; }
        public string Hash { get; set; }
    }
}
