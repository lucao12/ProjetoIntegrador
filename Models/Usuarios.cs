namespace ProjetoIntegrador.Models
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string? Sexo { get; set; }
        public int? Idade { get; set; }
        public double? Peso { get; set; }
        public double? Altura { get; set; }
        public string Role { get; set; }
        public string Telefone { get; set; }
        public string? Instagram { get; set; }
        public string? Foto { get; set; }
        public string Hash { get; set; }
    }
}
