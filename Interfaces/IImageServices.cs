namespace ProjetoIntegrador.Interfaces
{
    public interface IImageServices
    {
        Task<string> UploadImageAsync(byte[] imageBytes);
    }
}
