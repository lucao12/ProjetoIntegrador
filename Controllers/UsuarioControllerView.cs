using Microsoft.AspNetCore.Mvc;

namespace ProjetoIntegrador.Controllers
{
    [ApiController]
    public class UsuarioControllerView : Controller
    {
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View("~/Views/Usuario/Login.cshtml");
        }
        [HttpGet]
        [Route("cadastro")]
        public IActionResult Cadastro()
        {
            return View("~/Views/Usuario/Cadastro.cshtml");
        }
        [HttpGet]
        [Route("Redirecionando")]
        public IActionResult Home()
        {
            return View("~/Views/Usuario/Redirecionando.cshtml");
        }
        [HttpGet]
        [Route("HomeUsuario")]
        public IActionResult HomeUsuario()
        {
            return View("~/Views/Usuario/HomeUsuario.cshtml");
        }
        [HttpGet]
        [Route("virtual-select.min.js")]
        public IActionResult VirtualSelect()
        {
            return View("~/Views/Usuario/virtual-select.min.js");
        }
        [HttpGet]
        [Route("HomeAdmin")]
        public IActionResult HomeAdmin()
        {
            return View("~/Views/Usuario/HomeAdmin.cshtml");
        }
        [HttpGet]
        [Route("HomeNutri")]
        public IActionResult HomeNutri()
        {
            return View("~/Views/Usuario/HomeNutri.cshtml");
        }
    }
}
