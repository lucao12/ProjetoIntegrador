namespace ProjetoIntegrador.Interfaces
{
    public interface IHashServices
    {
        string GenerateHash(string password, byte[] salt);
        byte[] GenerateSalt();
    }
}
