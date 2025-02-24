using System.Net.Http.Headers;
using Newtonsoft.Json;
using ProjetoIntegrador.Interfaces;

namespace ProjetoIntegrador.Services
{
    public class ImageServices : IImageServices
    {
        private const string IMGUR_CLIENT_ID = "4b39b78f1880bf4";
        private const string IMGUR_UPLOAD_URL = "https://api.imgur.com/3/upload";

        public async Task<string> UploadImageAsync(byte[] imageBytes)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", IMGUR_CLIENT_ID);

                using (var content = new MultipartFormDataContent())
                {
                    var imageContent = new ByteArrayContent(imageBytes);
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    content.Add(imageContent, "image");

                    var response = await client.PostAsync(IMGUR_UPLOAD_URL, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Erro ao enviar imagem para Imgur: {responseString}");
                    }

                    var imgurResponse = JsonConvert.DeserializeObject<ImgurResponse>(responseString);

                    return imgurResponse?.Data?.Link ?? throw new Exception("Erro ao processar resposta do Imgur.");
                }
            }
        }
    }

    public class ImgurResponse
    {
        [JsonProperty("data")]
        public ImgurData Data { get; set; }
    }

    public class ImgurData
    {
        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
