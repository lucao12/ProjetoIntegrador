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
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        [Route("/candidatarNutri/{id:int}")]
        public async Task<IActionResult> VerificaPedidos(
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

                var nutri = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (nutri == null)
                {
                    return NotFound(new { error = "Nutri não encontrado" });
                }



                var newPedido = new UserNutri
                {
                    Aceito = false,
                    Nutricionista = nutri,
                    Usuario = usuario
                };

                await _context.Pedidos.AddAsync(newPedido);
                await _context.SaveChangesAsync();
                return Created($"{newPedido.Id}", newPedido);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
        [Authorize]
        [HttpPost]
        [Route("/validar/pedido/{pedidoId:int}/{aceito:int}")]
        public async Task<IActionResult> VerificaPedidosNutri(
            [FromRoute] int pedidoId,
            [FromRoute] int aceito)
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

                var pedido = await _context.Pedidos.FirstOrDefaultAsync(x => x.Id == pedidoId);

                if (pedido == null) {
                    return BadRequest(new
                    {
                        error = "Não há pedido com esse Id"
                    });
                }


                if(aceito == 1)
                {
                    pedido.Aceito = true;
                }
                else
                {
                    _context.Pedidos.Remove(pedido);
                }

                await _context.SaveChangesAsync();
                return Ok(new
                {
                    pedido = "Feito"
                });
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("verifica/pedidos")]
        public async Task<IActionResult> VerificaPedidos()
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

                var pedido = await _context.Pedidos
                    .Include(p => p.Usuario)
                    .Include(p => p.Nutricionista)
                    .FirstOrDefaultAsync(x => x.Usuario.Id == usuario.Id);

                if(pedido == null)
                {
                    return Ok(new
                    {
                        nao = "nao tem"
                    });
                }

                return Ok(new
                {
                    pedido = pedido
                });
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("verifica/pedidos/nutri")]
        public async Task<IActionResult> VerificaPedidosNutri()
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

                var pedido = await _context.Pedidos
                    .Include(p => p.Usuario)
                    .Where(x => x.Nutricionista.Id == usuario.Id && x.Aceito == false).ToListAsync();

                if (pedido == null || pedido.Count == 0)
                {
                    return Ok(new
                    {
                        nao = "nao tem"
                    });
                }

                return Ok(new
                {
                    pedido = pedido
                });
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("verifica/aceitos/nutri")]
        public async Task<IActionResult> VerificaPedidosAceitosNutri()
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

                var pedido = await _context.Pedidos
                    .Include(p => p.Usuario)
                    .Where(x => x.Nutricionista.Id == usuario.Id && x.Aceito == true).ToListAsync();

                if (pedido == null || pedido.Count == 0)
                {
                    return Ok(new
                    {
                        nao = "nao tem"
                    });
                }

                return Ok(new
                {
                    pedido = pedido
                });
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
    }
}
