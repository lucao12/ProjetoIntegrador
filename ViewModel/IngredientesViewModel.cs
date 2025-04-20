using System.ComponentModel.DataAnnotations;

namespace ProjetoIntegrador.ViewModel
{
    public class IngredientesViewModel
    {
        public string ingrediente { get; set; }
    }
    public class ReceitasViewModel
    {
        public List<string> ingrediente { get; set; }
    }

    public class AddIngredientesViewModel
    {
        public string nome { get; set; }
        public int calorias { get; set; }
        public int carboidratos { get; set; }
        public int proteinas { get; set; }
        public int gorduras { get; set; }
        public string vitaminaseminerais { get; set; }
    }
}
