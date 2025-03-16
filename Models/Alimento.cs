namespace ProjetoIntegrador.Models
{
    public class Alimento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? Calorias { get; set; }
        public int? Carboidratos { get; set; }
        public int? Proteinas { get; set; }
        public int? Gorduras { get; set; }
        public string? VitaminasEMinerais { get; set; }


    }
}