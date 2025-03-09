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
        public async Task<IActionResult> PegarMensagem(
            [FromRoute] int id)
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
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (recebe == null)
                {
                    return NotFound(new { error = "Nutri não encontrado" });
                }

                var mensagens = await _context.Mensagem
                .Where(m => ( m.Usuario == usuario || m.Usuario == recebe) && (m.Nutricionista == usuario || m.Nutricionista == recebe))
                .ToListAsync();
                if (recebe == null)
                {
                    return NotFound(new { error = "Mensagens não encontrado" });
                }

                return Ok(mensagens);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
    }
}
