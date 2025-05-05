public class Sintoma
{
    public int Id { get; set; }  // ID do sintoma (auto-incrementado no banco de dados)
    public int UsuarioId { get; set; }  // ID do usuário que relatou o sintoma
    public string SintomaDescricao { get; set; } 
    public List<string> Dias { get; set; }
}
