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

    [ApiController]
    public class DietaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DietaController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        [Route("/enviardieta")]
        public async Task<IActionResult> EnviarDieta(
            [FromBody] DietaViewModel model)
        {
            try
            {
                List<AlimentoQuantidade> cafe = new List<AlimentoQuantidade>();
                List<AlimentoQuantidade> almoco = new List<AlimentoQuantidade>();
                List<AlimentoQuantidade> cafedt = new List<AlimentoQuantidade>();
                List<AlimentoQuantidade> janta = new List<AlimentoQuantidade>();

                var userId = User.FindFirstValue(ClaimTypes.Email);
                var nutri = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == userId);


                if (nutri == null)
                {
                    return NotFound(new { error = "Usuário não encontrado" });
                }
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == model.idUsuario);

                if (usuario == null)
                {
                    return NotFound(new { error = "Usuário não encontrado" });
                }

                foreach (var item in model.Cafe)
                {
                    var alimento = await _context.Alimentos
                    .FirstOrDefaultAsync(u => u.Id == item.Key);
                    if (alimento == null)
                    {
                        return NotFound(new { error = "Usuário não encontrado" });
                    }
                    var AlimentosQuantidade = new AlimentoQuantidade
                    {
                        Alimentos = alimento,
                        Quantidade = item.Value
                    };
                    _context.AlimentoQuantidade.Add(AlimentosQuantidade);
                    await _context.SaveChangesAsync();
                    cafe.Add(AlimentosQuantidade);
                    
                }
                foreach (var item in model.Almoco)
                {
                    var alimento = await _context.Alimentos
                    .FirstOrDefaultAsync(u => u.Id == item.Key);
                    if (alimento == null)
                    {
                        return NotFound(new { error = "Usuário não encontrado" });
                    }
                    var AlimentosQuantidade = new AlimentoQuantidade
                    {
                        Alimentos = alimento,
                        Quantidade = item.Value
                    };
                    _context.AlimentoQuantidade.Add(AlimentosQuantidade);
                    await _context.SaveChangesAsync();
                    almoco.Add(AlimentosQuantidade);

                }
                foreach (var item in model.CafeDT)
                {
                    var alimento = await _context.Alimentos
                    .FirstOrDefaultAsync(u => u.Id == item.Key);
                    if (alimento == null)
                    {
                        return NotFound(new { error = "Usuário não encontrado" });
                    }
                    var AlimentosQuantidade = new AlimentoQuantidade
                    {
                        Alimentos = alimento,
                        Quantidade = item.Value
                    };
                    _context.AlimentoQuantidade.Add(AlimentosQuantidade);
                    await _context.SaveChangesAsync();
                    cafedt.Add(AlimentosQuantidade);

                }
                foreach (var item in model.Janta)
                {
                    var alimento = await _context.Alimentos
                    .FirstOrDefaultAsync(u => u.Id == item.Key);
                    if (alimento == null)
                    {
                        return NotFound(new { error = "Usuário não encontrado" });
                    }
                    var AlimentosQuantidade = new AlimentoQuantidade
                    {
                        Alimentos = alimento,
                        Quantidade = item.Value
                    };
                    _context.AlimentoQuantidade.Add(AlimentosQuantidade);
                    await _context.SaveChangesAsync();
                    janta.Add(AlimentosQuantidade);

                }
                var dieta = new Dieta
                {
                    Usuario = usuario,
                    Nutricionista = nutri,
                    Cafe = cafe,
                    Almoco = almoco,
                    CafeDT = cafedt,
                    Janta = janta
                };
                _context.Dieta.Add(dieta);
                await _context.SaveChangesAsync();
                return Ok(dieta);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }


        [Authorize]
        [HttpGet]
        [Route("/receberDieta")]
        public async Task<IActionResult> GetDieta()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.Email);



                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == userId);

                if(usuario == null)
                {
                    return BadRequest(new
                    {
                        erro = "Usuário não encontrado."
                    });
                }


                var dieta = await _context.Dieta
                    .Include(x => x.Usuario)
                    .Include(x => x.Nutricionista)
                    .Include(x => x.Cafe)
                        .ThenInclude(x => x.Alimentos) // Carregar alimentos de cafe
                    .Include(x => x.Almoco)
                        .ThenInclude(x => x.Alimentos) // Carregar alimentos de almoco
                    .Include(x => x.CafeDT)
                        .ThenInclude(x => x.Alimentos) // Carregar alimentos de cafeDT
                    .Include(x => x.Janta)
                        .ThenInclude(x => x.Alimentos) // Carregar alimentos de janta
                    .FirstOrDefaultAsync(x => x.Usuario.Id == usuario.Id);


                if (dieta == null)
                {
                    return BadRequest(new
                    {
                        erro = "Dieta não encontrada."
                    });
                }

                return Ok(dieta);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }

    }
}
