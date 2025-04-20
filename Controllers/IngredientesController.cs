using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoIntegrador.Data;
using ProjetoIntegrador.Interfaces;
using ProjetoIntegrador.Models;
using ProjetoIntegrador.ViewModel;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


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
                string prompt = $"Forneça apenas os valores de macronutrientes, micronutrientes e calorias em 100 gramas de {model.ingrediente}. Responda no formato: \nCalorias: X kcal\nCarboidratos: Y g\nProteínas: Z g\nGorduras: W g\nVitaminas e Minerais: (lista de principais nutrientes). Não use nenhum tipo de acento" ;
                
                string response = await SendRequestToGemini(prompt);

                return Ok(StringToJson(response));
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
        [Authorize]
        [HttpPost]
        [Route("/Receitas")]
        public async Task<IActionResult> Receitas(
        [FromBody] ReceitasViewModel model)
        {
            try
            {
                string prompt = $"Forneça apenas receitas completas, mostrando cada passo a passo do preparo com *APENAS* estes ingredientes ";
                foreach (string i in model.ingrediente)
                {
                    prompt += i + " ";
                }
                prompt += ".Não pode conter nenhum outro igrediente. Não use nenhum tipo de acento. Será usado em um programa então retorne em html, sem head, sem !DOCTYPE html e sem body só os hs e lis. Não use h1 e h2 use h6 e h7. GARANTA QUE USE OS H E LI. NO MAXIMO 2 RECEITAS. TODO RECEITAS DEVEM SER PARA O CONSUMO.E TIRE O ```html DO COMEÇO e ``` DO FIM ";

                string response = await SendRequestToGemini(prompt);
                response = response.Replace("```html", "").Replace("```", "").Trim();
                // Retorna HTML como texto puro
                return Content(response, "text/html");
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("/SalvarIngrediente")]
        public async Task<IActionResult> SalvarIngredienteAsync(
            [FromBody] AddIngredientesViewModel model)
        {
            try
            {
                var alimentoExiste = await _context.Alimentos.FirstOrDefaultAsync(x => x.Nome.ToLower() == model.nome);
                if(alimentoExiste != null)
                {
                    return BadRequest(new
                    {
                        erro = "Alimento já cadastrado!"
                    });
                }
                var newIngrediente = new Alimento
                {
                    Nome = model.nome,
                    Calorias = model.calorias,
                    Carboidratos = model.carboidratos,
                    Gorduras = model.gorduras,
                    Proteinas = model.proteinas,
                    VitaminasEMinerais = model.vitaminaseminerais
                };

                await _context.Alimentos.AddAsync(newIngrediente);
                await _context.SaveChangesAsync();

                return Ok();
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
        public static string StringToJson(string nutritionString)
        {
            var nutritionData = new Dictionary<string, object>(); // Use object to handle different value types

            string[] lines = nutritionString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string valueString = parts[1].Trim();

                    // Try to parse value as number (int or float), otherwise treat as string
                    if (double.TryParse(valueString.Split(' ')[0], out double numericValue)) // Handle units like "89 kcal"
                    {
                        nutritionData[key] = numericValue;
                    }
                    else
                    {
                        nutritionData[key] = valueString;
                    }
                }
            }

            string jsonString = System.Text.Json.JsonSerializer.Serialize(nutritionData, new JsonSerializerOptions { WriteIndented = true }); // Pretty print JSON
            return jsonString;
        }

    }
}
