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
                
                List<Dieta> DietaList = new List<Dieta>();
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
                foreach (var itens in model.Dietas)
                {
                    List<AlimentoQuantidade> cafe = new List<AlimentoQuantidade>();
                    List<AlimentoQuantidade> almoco = new List<AlimentoQuantidade>();
                    List<AlimentoQuantidade> cafedt = new List<AlimentoQuantidade>();
                    List<AlimentoQuantidade> janta = new List<AlimentoQuantidade>();
                    foreach (var item in itens.Cafe)
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
                    foreach (var item in itens.Almoco)
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
                    foreach (var item in itens.CafeDT)
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
                    foreach (var item in itens.Janta)
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
                   
                    var dietaLista = new Dieta
                    {
                        Cafe = cafe,
                        Almoco = almoco,
                        CafeDT = cafedt,
                        Janta = janta
                    };
                    _context.Dieta.Add(dietaLista);
                    Console.WriteLine(dietaLista);
                    DietaList.Add(dietaLista);
                    await _context.SaveChangesAsync();
                }
                var dieta = new DietaSemana
                {
                    Usuario = usuario,
                    Nutricionista = nutri,
                    Dietas = DietaList
                };
                _context.DietaSemana.Add(dieta);
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

                if (usuario == null)
                {
                    return BadRequest(new
                    {
                        erro = "Usuário não encontrado."
                    });
                }

                var dietaSemana = await _context.DietaSemana.Include(x => x.Usuario)
                    .Include(x => x.Nutricionista)
                    .Include(x => x.Dietas)
                        .ThenInclude(x => x.Cafe)
                        .ThenInclude(x => x.Alimentos)
                    .Include(x => x.Dietas)
                        .ThenInclude(x => x.Almoco)
                        .ThenInclude(x => x.Alimentos)
                    .Include(x => x.Dietas)
                        .ThenInclude(x => x.CafeDT)
                        .ThenInclude(x => x.Alimentos)
                    .Include(x => x.Dietas)
                        .ThenInclude(x => x.Janta)
                        .ThenInclude(x => x.Alimentos)
                    .FirstOrDefaultAsync(x => x.Usuario.Id == usuario.Id);



                if (dietaSemana == null)
                {
                    return BadRequest(new
                    {
                        erro = "Dieta não encontrada."
                    });
                }

                return Ok(dietaSemana);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }

    }
}
