using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Data;
using ProjetoIntegrador.Interfaces;
using ProjetoIntegrador.Models;
using ProjetoIntegrador.ViewModel;
using System.Security.Claims;

namespace ProjetoIntegrador.Controllers
{
    [ApiController]
    public class MensagemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MensagemController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        [Route("/EnviarMensagem")]
        public async Task<IActionResult> EnviarMensagem(
            [FromBody] EnvioMensagemViewModel model)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.Email);
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == userId);


                if (usuario == null)
                {
                    return NotFound(new { error = "Usuário não encontrado" });
                }

                var recebe = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == model.id);

                if (recebe == null)
                {
                    return NotFound(new { error = "Nutri não encontrado" });
                }


                var newMensagem = new Mensagem
                {
                    Usuario = usuario,
                    Nutricionista = recebe,
                    Mensage = model.mensagem
                };

                await _context.Mensagem.AddAsync(newMensagem);
                await _context.SaveChangesAsync();
                return Created($"{newMensagem.Id}", newMensagem);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
        
        [Authorize]
        [HttpGet]
        [Route("/PegarMensagem/{id:int}")]
        public async Task<IActionResult> PegarMensagem([FromRoute] int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.Email);
                Console.WriteLine($"Usuário autenticado: {userId}");

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == userId);

                if (usuario == null)
                {
                    Console.WriteLine("Usuário não encontrado no banco de dados.");
                    return NotFound(new { error = "Usuário não encontrado" });
                }

                var recebe = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (recebe == null)
                {
                    Console.WriteLine("Nutricionista não encontrado no banco de dados.");
                    return NotFound(new { error = "Nutri não encontrado" });
                }

                var mensagensUsuario = await _context.Mensagem
                    .Where(m => m.Usuario.Id == usuario.Id && m.Nutricionista.Id == recebe.Id)
                    .Select(m => new 
                    {
                        m.Id,
                        Remetente = "Usuário",
                        RemetenteId = m.Usuario.Id, // Id do remetente (usuário)
                        Mensagem = m.Mensage
                    })
                    .ToListAsync();

                var mensagensNutricionista = await _context.Mensagem
                    .Where(m => m.Usuario.Id == recebe.Id && m.Nutricionista.Id == usuario.Id)
                    .Select(m => new 
                    {
                        Id = m.Id,
                        Remetente = "Nutricionista",
                        RemetenteId = m.Usuario.Id, // Id do remetente (nutricionista)
                        Mensagem = m.Mensage
                    })
                    .ToListAsync();

                Console.WriteLine($"Mensagens do usuário: {mensagensUsuario.Count}");
                Console.WriteLine($"Mensagens do nutricionista: {mensagensNutricionista.Count}");

                var resultado = new
                {
                    mensagensUsuario,
                    mensagensNutricionista
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro interno: {ex.Message}");
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
    }
}
