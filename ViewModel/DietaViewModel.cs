using System.ComponentModel.DataAnnotations;

namespace ProjetoIntegrador.ViewModel
{
    public class DietaViewModel
    {
        public int idUsuario { get; set; }
        public List<KeyValuePair<int, int>> Cafe { get; set; }
        public List<KeyValuePair<int, int>> Almoco { get; set; }
        public List<KeyValuePair<int, int>> CafeDT { get; set; }
        public List<KeyValuePair<int, int>> Janta { get; set; }
    }
}
