namespace ProjetoIntegrador.ViewModel
{
    public class EmailViewModelEnviar
    {
        public int idusuario { get; set; }
        public string titulo { get; set; }
        public string body { get; set; }
    }
    public class EmailViewModelEnviarAnexo
    {
        public int idusuario { get; set; }
        public string titulo { get; set; }
        public string body { get; set; }
        public string anexoBase64 { get; set; } 
        public string nomeArquivo { get; set; }
    }
}
