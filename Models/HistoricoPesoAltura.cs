namespace ProjetoIntegrador.Models
{
    public class HistoricoPesoAltura
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public double? Peso { get; set; }
        public double? Altura { get; set; }
        public DateTime DataAlteracao { get; set; }
        public Usuarios Usuario { get; set; }
    }
}