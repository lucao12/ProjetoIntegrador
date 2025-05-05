using System.ComponentModel.DataAnnotations;

namespace ProjetoIntegrador.ViewModel
{
    public class DietasList
    {
        public string Dia { get; set; } // <-- aqui
        public List<KeyValuePair<int, int>>? Cafe { get; set; }
        public List<KeyValuePair<int, int>>? Almoco { get; set; }
        public List<KeyValuePair<int, int>>? CafeDT { get; set; }
        public List<KeyValuePair<int, int>>? Janta { get; set; }
    }
    public class DietaViewModel
    {
        public int idUsuario { get; set; }
        public List<DietasList> Dietas { get; set; }
        public double MetaPeso { get; set; } // <- NOVO CAMPO

    }
}
