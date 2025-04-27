namespace ProjetoIntegrador.ViewModel
{
    public class LayoutListViewModel
    {
        public List<KeyValuePair<int, int>>? Cafe { get; set; }
        public List<KeyValuePair<int, int>>? Almoco { get; set; }
        public List<KeyValuePair<int, int>>? CafeDT { get; set; }
        public List<KeyValuePair<int, int>>? Janta { get; set; }
    }
    public class LayoutCreateViewModel
    {
        public string Name { get; set; }
        public LayoutListViewModel Dietas { get; set; }

    }
}
