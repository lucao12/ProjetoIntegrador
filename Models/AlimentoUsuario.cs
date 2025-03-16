namespace ProjetoIntegrador.Models
{
    public class AlimentoUsuario
    {
        public int UsuarioId { get; set; }
        public Usuarios Usuario { get; set; }

        public int AlimentoId { get; set; }
        public Alimento Alimento { get; set; }
    }


}
