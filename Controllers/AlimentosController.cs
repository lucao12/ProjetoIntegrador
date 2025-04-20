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
using System.Text.Json;

namespace ProjetoIntegrador.Controllers
{


    [Route("/alimentos")]
    [ApiController]
    public class AlimentosController : ControllerBase
    {
        private static readonly string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=AIzaSyCiiHiVffHDhMcNt8fY3rYl5EBqsB00l6E";
        private readonly AppDbContext _context;

        public AlimentosController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("getAlimentos")]
        public async Task<IActionResult> GetAlimentos()
        {
            try
            {
                var alimentos = await _context.Alimentos
                    .Select(a => new { a.Id, a.Nome })
                    .ToListAsync();

                if (alimentos == null || !alimentos.Any()) // Se a lista estiver vazia
                {
                    return Ok(new List<object>()); // Retorna um JSON vazio []
                }

                return Ok(alimentos);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
        [Authorize]
        [HttpGet("getAlimentosid/{alimentoId:int}")]
        public async Task<IActionResult> GetAlimentosbyId([FromRoute] int alimentoId)
        {
            try
            {
                var alimento = await _context.Alimentos
                    .Where(a => a.Id == alimentoId)
                    .Select(a => new { a.Id, a.Nome })
                    .FirstOrDefaultAsync();

                if (alimento == null) // Se o alimento não for encontrado
                {
                    return NotFound(new { error = "Alimento não encontrado!" }); // Retorna erro 404
                }

                return Ok(alimento); // Retorna o alimento encontrado
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" }); // Retorna erro 500
            }
        }
        [Authorize]
        [HttpPost("salvarAlimentos")]
        public async Task<IActionResult> SalvarAlimentos([FromBody] AlimentoUsuarioViewModel dados)
        {
            // Verificando se o array de AlimentosIds está vazio
            if (dados.AlimentoIds == null || dados.AlimentoIds.Count == 0)
            {
                // Se estiver vazio, removemos todas as relações do usuário com os alimentos
                var alimentosUsuario = await _context.AlimentosUsuarios
                    .Where(a => a.UsuarioId == dados.UsuarioId)
                    .ToListAsync();

                if (alimentosUsuario.Any())
                {
                    _context.AlimentosUsuarios.RemoveRange(alimentosUsuario); // Remover todos os registros associados
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Alimentos removidos com sucesso!" });
                }
                else
                {
                    return NotFound(new { message = "Nenhum alimento encontrado para o usuário." });
                }
            }

            // Caso contrário, adicionamos os alimentos selecionados ao usuário
            var alimentosUsuarioExistentes = await _context.AlimentosUsuarios
                .Where(a => a.UsuarioId == dados.UsuarioId)
                .ToListAsync();

            // Remover alimentos que não foram selecionados
            foreach (var alimentoUsuario in alimentosUsuarioExistentes)
            {
                if (!dados.AlimentoIds.Contains(alimentoUsuario.AlimentoId))
                {
                    _context.AlimentosUsuarios.Remove(alimentoUsuario);
                }
            }

            // Adicionar os alimentos novos ao usuário
            foreach (var alimentoId in dados.AlimentoIds)
            {
                if (!alimentosUsuarioExistentes.Any(a => a.AlimentoId == alimentoId))
                {
                    var alimentoUsuario = new AlimentoUsuario
                    {
                        UsuarioId = dados.UsuarioId,
                        AlimentoId = alimentoId
                    };
                    _context.AlimentosUsuarios.Add(alimentoUsuario);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Alimentos atualizados com sucesso!" });
        }

        [Authorize]
        [HttpGet("getAlimentosUsuario/{usuarioId}")]
        public async Task<IActionResult> GetAlimentosUsuario(int usuarioId)
        {
            // Verificando se o usuário existe
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado!" });
            }

            // Buscando os alimentos associados ao usuário
            var alimentos = await _context.AlimentosUsuarios
                .Where(a => a.UsuarioId == usuarioId)
                .Select(a => a.Alimento)
                .ToListAsync();

            return Ok(alimentos);  // Retorna os alimentos encontrados, se houver
        }

        [Authorize]
        [HttpPost]
        [Route("/SubstituirAlimentos")]
        public async Task<IActionResult> SubstituirAlimentos(
            [FromBody] SubstituirAlimentosViewModel model)
        {
            try
            {
                string prompt = $"Forneça alimentos parecido em valores de macronutrientes, micronutrientes e calorias do alimento {model.alimento}, para uma refeição de {model.refeicao}. Retorne apenas com o nome do alimento. Mande no maximo 5 opções . E só isso, não coloque nenhuma outra informação. Não use nenhum tipo de acento.";

                string response = await SendRequestToGemini(prompt);

                return Ok(StringToJson(response));
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
            // Inicializa uma lista para armazenar os objetos de alimentos
            var alimentosList = new List<object>();

            // Divide a string recebida por linha
            string[] alimentos = nutritionString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Adiciona um objeto para cada alimento na lista
            foreach (var alimento in alimentos)
            {
                alimentosList.Add(new { nome = alimento.Trim() });
            }

            // Converte a lista de objetos para JSON e retorna como string
            string jsonString = System.Text.Json.JsonSerializer.Serialize(alimentosList, new JsonSerializerOptions { WriteIndented = true });
            return jsonString;
        }

    }
}