using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Data;
using ProjetoIntegrador.Models;
using ProjetoIntegrador.ViewModel;

namespace ProjetoIntegrador.Controllers
{
    [ApiController]
    public class LayoutController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LayoutController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost]
        [Route("/enviarLayout")]
        public async Task<IActionResult> EnviarLayout(
            [FromBody] LayoutCreateViewModel model)
        {
            try
            {

                var userId = User.FindFirstValue(ClaimTypes.Email);
                var nutri = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == userId);


                if (nutri == null)
                {
                    return NotFound(new { error = "Usuário não encontrado1" });
                }

                    List<AlimentoLayout> cafe = new List<AlimentoLayout>();
                    List<AlimentoLayout> almoco = new List<AlimentoLayout>();
                    List<AlimentoLayout> cafedt = new List<AlimentoLayout>();
                    List<AlimentoLayout> janta = new List<AlimentoLayout>();
                    foreach (var item in model.Dietas.Cafe)
                    {
                        var alimento = await _context.Alimentos
                        .FirstOrDefaultAsync(u => u.Id == item.Key);
                        if (alimento == null)
                        {
                            return NotFound(new { error = "Usuário não encontrado3" });
                        }
                        var AlimentosQuantidade = new AlimentoLayout
                        {
                            Alimentos = alimento,
                            Quantidade = item.Value
                        };
                        _context.AlimentoLayout.Add(AlimentosQuantidade);
                        await _context.SaveChangesAsync();
                        cafe.Add(AlimentosQuantidade);
                    }
                    foreach (var item in model.Dietas.Almoco)
                    {
                        var alimento = await _context.Alimentos
                        .FirstOrDefaultAsync(u => u.Id == item.Key);
                        if (alimento == null)
                        {
                            return NotFound(new { error = "Usuário não encontrado4" });
                        }
                        var AlimentosQuantidade = new AlimentoLayout
                        {
                            Alimentos = alimento,
                            Quantidade = item.Value
                        };
                        _context.AlimentoLayout.Add(AlimentosQuantidade);
                        await _context.SaveChangesAsync();
                        almoco.Add(AlimentosQuantidade);

                    }
                    foreach (var item in model.Dietas.CafeDT)
                    {
                        var alimento = await _context.Alimentos
                        .FirstOrDefaultAsync(u => u.Id == item.Key);
                        if (alimento == null)
                        {
                            return NotFound(new { error = "Usuário não encontrado5" });
                        }
                        var AlimentosQuantidade = new AlimentoLayout
                        {
                            Alimentos = alimento,
                            Quantidade = item.Value
                        };
                        _context.AlimentoLayout.Add(AlimentosQuantidade);
                        await _context.SaveChangesAsync();
                        cafedt.Add(AlimentosQuantidade);

                    }
                    foreach (var item in model.Dietas.Janta)
                    {
                        var alimento = await _context.Alimentos
                        .FirstOrDefaultAsync(u => u.Id == item.Key);
                        if (alimento == null)
                        {
                            return NotFound(new { error = "Usuário não encontrado6" });
                        }
                        var AlimentosQuantidade = new AlimentoLayout
                        {
                            Alimentos = alimento,
                            Quantidade = item.Value
                        };
                        _context.AlimentoLayout.Add(AlimentosQuantidade);
                        await _context.SaveChangesAsync();
                        janta.Add(AlimentosQuantidade);

                    }

                    var dietaLista = new Layout
                    {
                        Name = model.Name,
                        Cafe = cafe,
                        Almoco = almoco,
                        CafeDT = cafedt,
                        Janta = janta,
                        Nutri = nutri,
                    };
                    _context.Layout.Add(dietaLista);
                    Console.WriteLine(dietaLista);
                    await _context.SaveChangesAsync();
                
                return Ok();
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("/Layouts")]
        public async Task<IActionResult> PegarLayout()
        {
            try
            {

                var userId = User.FindFirstValue(ClaimTypes.Email);
                var nutri = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == userId);


                if (nutri == null)
                {
                    return NotFound(new { error = "Usuário não encontrado1" });
                }

                var layouts = await _context.Layout
                    .Include(x => x.Nutri)
                    .Include(x => x.Cafe)
                        .ThenInclude(x=>x.Alimentos)
                    .Include(x => x.Almoco)
                        .ThenInclude(x => x.Alimentos)
                    .Include(x => x.CafeDT)
                        .ThenInclude(x => x.Alimentos)
                    .Include(x => x.Janta)
                        .ThenInclude(x => x.Alimentos)
                    .Where(x => x.Nutri.Id == nutri.Id)
                    .ToListAsync();

                return Ok(layouts);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
    }
}
