using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoIntegrador.Data;
using ProjetoIntegrador.Interfaces;
using ProjetoIntegrador.Models;
using ProjetoIntegrador.ViewModel;
using System.Security.Claims;
using System.Text;


namespace ProjetoIntegrador.Controllers
{

    [ApiController]
    public class IngredientesController : ControllerBase
    {
        private static readonly string apiKey = "AIzaSyCiiHiVffHDhMcNt8fY3rYl5EBqsB00l6E";
        private static readonly string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";
        private readonly AppDbContext _context;

        public IngredientesController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        [Route("/EnviarIngrediente")]
        public async Task<IActionResult> EnviarMensagem(
            [FromBody] IngredientesViewModel model)
        {
            try
            {
                string prompt = $"Forneça apenas os valores de macronutrientes, micronutrientes e calorias em 100 gramas de {model.ingrediente}. Responda no formato: \nCalorias: X kcal\nCarboidratos: Y g\nProteínas: Z g\nGorduras: W g\nVitaminas e Minerais: (lista de principais nutrientes)";
                string response = await SendRequestToGemini(prompt);

                return Ok(response);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
        static async Task<string> SendRequestToGemini(string prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
                };

                string jsonBody = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string responseString = await response.Content.ReadAsStringAsync();

                dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                return jsonResponse?.candidates?[0]?.content?.parts?[0]?.text ?? "Nenhuma resposta recebida.";
            }
        }

    }
}
