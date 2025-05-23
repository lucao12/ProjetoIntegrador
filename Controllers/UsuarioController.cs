﻿using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Data;
using ProjetoIntegrador.Interfaces;
using ProjetoIntegrador.Models;
using Microsoft.AspNetCore.Mvc;
using ProjetoIntegrador.ViewModel;
using ProjetoIntegrador.Services;

namespace ProjetoIntegrador.Controllers

{
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHashServices _hashServices;
        private readonly ITokenServices _tokenServices;
        private readonly IImageServices _imageServices;
        public UsuarioController(AppDbContext context, IHashServices hashServices, ITokenServices tokenServices, IImageServices imageServices)
        {
            _context = context;
            _hashServices = hashServices;
            _tokenServices = tokenServices;
            _imageServices = imageServices;
        }
        [HttpPost]
        [Route(template: "login")]
        public async Task<IActionResult> LoginAsync(
            [FromBody] UsuarioLoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userExist = await _context.Usuarios.FirstOrDefaultAsync(x =>
                x.Email == model.Email.ToLower());

                if (userExist == null)
                {
                    return BadRequest(new
                    {
                        error = "Email e/ou senha incorretos!"
                    });
                }

                var user = await _context.Usuarios.FirstOrDefaultAsync(x =>
                x.Email == model.Email.ToLower() &&
                x.Senha == _hashServices.GenerateHash(model.Senha, Convert.FromBase64String(userExist.Hash)));

                if (user == null)
                {
                    return BadRequest(new
                    {
                        error = "Email e/ou senha incorretos!"
                    });
                }

                var token = _tokenServices.GenerateToken(user);
                return Ok(new
                {
                    user = user,
                    token = token
                });
            }
            catch
            {
                return StatusCode(500, new
                {
                    error = "Erro interno do servidor!"
                });
            }
        }
        [HttpPost]
        [Route(template: "signup")]
        public async Task<IActionResult> SignUpAsync(
            [FromBody] UsuarioCadastroViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.ToLower() == model.Email.ToLower());

                if (user != null)
                {
                    return BadRequest(new { error = "Usuário já cadastrado!" });
                }

                var salt = _hashServices.GenerateSalt();

                var hashedPassword = _hashServices.GenerateHash(model.Senha, salt);

                var newUser = new Usuarios
                {
                    Nome = model.Nome,
                    Email = model.Email.ToLower(),
                    Senha = hashedPassword,
                    Role = "User",
                    Hash = Convert.ToBase64String(salt),
                    Telefone = model.Telefone ?? "",
                    Altura = null,
                    Idade = null,
                    Peso = null,
                    Sexo = null,
                    Foto = null,
                    Instagram = null
                };

                await _context.Usuarios.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return Created($"{newUser.Id}", newUser);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
        [Authorize]
        [HttpPost]
        [Route(template: "mudasenha")]
        public async Task<IActionResult> MudaSenhaAsync(
            [FromBody] UsuarioMudaSenhaViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userFind = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == userEmail);
                var id = userFind.Id;
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return BadRequest(new
                    {
                        error = "Usuário não encontrado!"
                    });
                }

                var user2 = await _context.AdmCriou.FirstOrDefaultAsync(x => x.User.Id == id);
                var userCriado = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                if (user2 == null || userCriado == null)
                {
                    return BadRequest(new { error = "Usuário não encontrado!" });
                }

                var salt = _hashServices.GenerateSalt();

                var hashedPassword = _hashServices.GenerateHash(model.Senha, salt);

                userCriado.Senha = hashedPassword;
                userCriado.Hash = Convert.ToBase64String(salt);

                _context.Usuarios.Update(userCriado);

                _context.AdmCriou.Remove(user2);
                await _context.SaveChangesAsync();
                return Ok("Senha alterada");
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route(template: "cadastroAdmin")]
        public async Task<IActionResult> CadastroAdmin(
            [FromBody] UsuarioCadastroAdminViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.ToLower() == model.Email.ToLower());

                if (user != null)
                {
                    return BadRequest(new { error = "Usuário já cadastrado!" });
                }

                var salt = _hashServices.GenerateSalt();

                var hashedPassword = _hashServices.GenerateHash(model.Senha, salt);

                var newUser = new Usuarios
                {
                    Nome = model.Nome,
                    Email = model.Email.ToLower(),
                    Senha = hashedPassword,
                    Role = model.Role,
                    Hash = Convert.ToBase64String(salt),
                    Telefone = model.Telefone ?? ""
                };

                var newAdm = new AdmCriou
                {
                    User = newUser,
                    Criou = true
                };

                await _context.Usuarios.AddAsync(newUser);
                await _context.AdmCriou.AddAsync(newAdm);
                await _context.SaveChangesAsync();
                return Created($"{newUser.Id}", newUser);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }

        [Authorize]
        [HttpGet]
        [Route(template: "verificaAdmCriou")]
        public async Task<IActionResult> GetVerificaAllAsync()
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userFind = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == userEmail);
                var id = userFind.Id;
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return BadRequest(new
                    {
                        error = "Usuário não encontrado!"
                    });
                }

                var admCriou = await _context.AdmCriou.FirstOrDefaultAsync(x => x.User.Id == user.Id && x.Criou == true);

                if (admCriou == null)
                {
                    return Ok(new
                    {
                        error = "Não"
                    });
                }

                return Ok(new
                {
                    Sim = "Sim"
                });
            }
            catch
            {
                return StatusCode(500, new
                {
                    error = "Erro interno do servidor!"
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route(template: "users")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var users = await _context.Usuarios.OrderBy(x => x.Role).ThenBy(x => x.Nome).ToListAsync();

                return Ok(users);
            }
            catch
            {
                return StatusCode(500, new
                {
                    error = "Erro interno do servidor!"
                });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("users/nutris")]
        public async Task<IActionResult> GetNutrisAsync()
        {
            try
            {
                var nutris = await _context.Usuarios
                                           .Where(x => x.Role == "Nutri")
                                           .OrderBy(x => x.Nome)
                                           .ToListAsync();

                return Ok(nutris);
            }
            catch
            {
                return StatusCode(500, new
                {
                    error = "Erro interno do servidor!"
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route(template: "user/yourself")]
        public async Task<IActionResult> GetYourselfAsync()
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userFind = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == userEmail);
                var id = userFind.Id;
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return BadRequest(new
                    {
                        error = "Usuário não encontrado!"
                    });
                }

                return Ok(user);
            }
            catch
            {
                return StatusCode(500, new
                {
                    error = "Erro interno do servidor!"
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route(template: "user/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id)
        {
            try
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return BadRequest(new
                    {
                        error = "Usuário não encontrado!"
                    });
                }

                return Ok(user);
            }
            catch
            {
                return StatusCode(500, new
                {
                    error = "Erro interno do servidor!"
                });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("pesoSemana/{id:int}")]
        public async Task<IActionResult> GetPesoAlturaAsync([FromRoute] int id)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == userEmail);

                if (user == null)
                {
                    return BadRequest(new { error = "Usuário não encontrado!" });
                }


                var pesos = await _context.HistoricoPesoAltura.Where(x => x.UsuarioId == id).ToListAsync();

                if (pesos.Count == 0)
                {
                    BadRequest(new { error = "usuário não possui histórico!" });
                }


                return Ok(pesos);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("role")]
        public async Task<IActionResult> GetRole()
        {
            try
            {
                var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(userRole))
                {
                    return Unauthorized(new
                    {
                        error = "Usuário sem permissão ou role não definida!"
                    });
                }

                return Ok(new
                {
                    role = userRole
                });
            }
            catch
            {
                return StatusCode(500, new
                {
                    error = "Erro interno do servidor!"
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route(template: "delete/user/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id)
        {
            try
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return BadRequest(new
                    {
                        error = "Usuário não encontrado!"
                    });
                }

                _context.Usuarios.Remove(user);
                await _context.SaveChangesAsync();

                return Ok("Usuário deletado com sucesso");
            }
            catch
            {
                return StatusCode(500, new
                {
                    error = "Erro interno do servidor!"
                });
            }
        }
        [Authorize]
        [HttpDelete]
        [Route(template: "delete/yourself")]
        public async Task<IActionResult> DeleteYourselfAsync()
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userFind = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == userEmail);
                var id = userFind.Id;
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return BadRequest(new
                    {
                        error = "Usuário não encontrado!"
                    });
                }

                var pedido = await _context.Pedidos.Include(p => p.Usuario).FirstOrDefaultAsync(x => x.Usuario.Id == user.Id);

                if (pedido != null)
                {
                    _context.Pedidos.Remove(pedido);
                }

                _context.Usuarios.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    sucesso = "Usuario deletado"
                });
            }
            catch
            {
                return StatusCode(500, new
                {
                    error = "Erro interno do servidor!"
                });
            }
        }
        [Authorize]
        [HttpPut]
        [Route(template: "update/user/yourself")]
        public async Task<IActionResult> UpdateAsync([FromBody] UsuarioUpdateViewModel model)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == userEmail);

                if (user == null)
                {
                    return BadRequest(new { error = "Usuário não encontrado!" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (model.Email != null)
                {
                    var verificaEmail = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == model.Email.ToLower());
                    if (verificaEmail != null && verificaEmail != user)
                    {
                        return BadRequest(new { error = "Email já cadastrado!" });
                    }
                }

                if (_hashServices.GenerateHash(model.SenhaAntiga, Convert.FromBase64String(user.Hash)) != user.Senha)
                {
                    return Unauthorized(new { error = "Senha inválida!" });
                }

                user.Nome = model.Nome ?? user.Nome;
                user.Email = model.Email ?? user.Email;
                user.Sexo = model.Sexo ?? user.Sexo;
                user.Idade = model.Idade ?? user.Idade;
                user.Peso = model.Peso ?? user.Peso;
                user.Altura = model.Altura ?? user.Altura;
                user.Senha = (model.NovaSenha != null && _hashServices.GenerateHash(model.NovaSenha, Convert.FromBase64String(user.Hash)) != user.Senha)
                    ? _hashServices.GenerateHash(model.NovaSenha, Convert.FromBase64String(user.Hash))
                    : user.Senha;
                user.Telefone = model.Telefone ?? user.Telefone;

                _context.Usuarios.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { sucesso = "Usuário atualizado com sucesso!" });
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
        [Authorize]
        [HttpPut]
        [Route("update/user/peso")]
        public async Task<IActionResult> UpdatePesoAlturaAsync([FromBody] UsuarioUpdateViewModel model)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == userEmail);

                if (user == null)
                {
                    return BadRequest(new { error = "Usuário não encontrado!" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Criar o novo histórico de peso e altura
                var historico = new HistoricoPesoAltura
                {
                    UsuarioId = user.Id,
                    Peso = model.Peso,
                    Altura = model.Altura,
                    DataAlteracao = DateTime.UtcNow // Definir a data e hora da alteração
                };

                // Adicionar o histórico na tabela HistoricoPesoAltura
                await _context.HistoricoPesoAltura.AddAsync(historico);

                // Atualizar a tabela Usuarios com o novo peso e altura
                user.Peso = model.Peso;
                user.Altura = model.Altura;

                // Atualizar o usuário na tabela de usuários
                _context.Usuarios.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { sucesso = "Peso e altura atualizados com sucesso!" });
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        [HttpPut]
        [Route("update/user/photo")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateUserPhoto([FromForm] IFormFile Foto)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == userEmail);

                if (user == null)
                {
                    return BadRequest(new { error = "Usuário não encontrado!" });
                }

                if (Foto == null || Foto.Length == 0)
                {
                    return BadRequest(new { error = "Nenhuma foto enviada!" });
                }

                // Converte a imagem para um array de bytes
                using (var memoryStream = new MemoryStream())
                {
                    await Foto.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();

                    // Enviar para Imgur (ou outro serviço)
                    var imageUrl = await _imageServices.UploadImageAsync(imageBytes);
                    user.Foto = imageUrl;
                }

                _context.Usuarios.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { sucesso = "Foto atualizada com sucesso!", fotoUrl = user.Foto });
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
        [Authorize(Roles = "Nutri")]
        [HttpPut]
        [Route(template: "update/user/social")]
        public async Task<IActionResult> UpdateSocialAsync(
            [FromBody] NutriSocailUpdateViewModel model)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userFind = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == userEmail);
                var id = userFind.Id;
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return BadRequest(new
                    {
                        error = "Usuário não encontrado!"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                user.Instagram = (model.Instagram != null && model.Instagram != user.Instagram) ? model.Instagram : user.Instagram;

                _context.Usuarios.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    sucesso = "Usuario atualizado"
                });
            }
            catch
            {
                return StatusCode(500, new
                {
                    error = "Erro interno do servidor!"
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route(template: "update/user/{id:int}")]
        public async Task<IActionResult> UpdateAdminAsync(
            [FromRoute] int id,
            [FromBody] UsuarioUpdateAdminViewModel model)
        {
            try
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return BadRequest(new
                    {
                        error = "Usuário não encontrado!"
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (model.Email != null)
                {
                    var verificaEmail = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == model.Email.ToLower());
                    if (verificaEmail != null && verificaEmail != user)
                    {
                        return BadRequest(new
                        {
                            error = "Email já cadastrado!"
                        });
                    }
                }

                bool verificador = false;

                if (model.Senha != null && _hashServices.GenerateHash(model.Senha, Convert.FromBase64String(user.Hash)) != user.Senha)
                {

                    verificador = true;
                }

                user.Nome = (model.Nome != null && model.Nome != user.Nome) ? model.Nome : user.Nome;
                user.Email = (model.Email != null && model.Email != user.Email) ? model.Email : user.Email;
                user.Senha = (model.Senha != null && _hashServices.GenerateHash(model.Senha, Convert.FromBase64String(user.Hash)) != user.Senha) ? _hashServices.GenerateHash(model.Senha, Convert.FromBase64String(user.Hash)) : user.Senha;
                user.Telefone = (model.Telefone != null && model.Telefone != user.Telefone) ? model.Telefone : user.Telefone;
                user.Role = (model.Role != null && model.Role != user.Role) ? model.Role : user.Role;

                if (verificador)
                {
                    var newAdm = new AdmCriou
                    {
                        User = user,
                        Criou = true
                    };

                    await _context.AdmCriou.AddAsync(newAdm);
                }

                _context.Usuarios.Update(user);
                await _context.SaveChangesAsync();

                return Ok("Usuário atualizado com sucesso");
            }
            catch
            {
                return StatusCode(500, new
                {
                    error = "Erro interno do servidor!"
                });
            }
        }

        [HttpPost]
        [Route(template: "esqueci")]
        public async Task<IActionResult> EsqueciAsync(
            [FromBody] UsuarioEsqueciViewModel model,
            [FromServices] EmailServices emailService)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.ToLower() == model.Email.ToLower());

                if (user == null)
                {
                    return BadRequest(new { error = "Usuário não cadastrado!" });
                }

                var random = new Random();
                var codigo = random.Next(100000, 999999);

                var newcode = new Codigos
                {
                    Codigo = codigo,
                    User = user
                };

                await emailService.EnviarEmailAsync(user.Email, "Código de verificação", $"Seu código de verificação é: {codigo}");

                await _context.Codigos.AddAsync(newcode);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensagem = "Sucesso"
                });
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }

        [HttpPost]
        [Route(template: "esqueciValida")]
        public async Task<IActionResult> EsqueciValidaAsync(
            [FromBody] UsuarioEsqueciValidaViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.ToLower() == model.Email.ToLower());

                if (user == null)
                {
                    return BadRequest(new { error = "Usuário não cadastrado!" });
                }

                var existe = await _context.Codigos.FirstOrDefaultAsync(x => x.User.Email.ToLower() == model.Email.ToLower() &&
                x.Codigo == model.Codigo);

                if (existe == null)
                {
                    return BadRequest(new { error = "Código inválido!" });
                }

                return Ok(new
                {
                    mensagem = "Sucesso"
                });
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }

        [HttpPost]
        [Route(template: "trocaSenha")]
        public async Task<IActionResult> TrocaSenhaAsync(
            [FromBody] UsuarioTrocaSenhaViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.ToLower() == model.Email.ToLower());

                if (user == null)
                {
                    return BadRequest(new { error = "Usuário não cadastrado!" });
                }

                var salt = _hashServices.GenerateSalt();

                var hashedPassword = _hashServices.GenerateHash(model.Senha, salt);

                user.Senha = hashedPassword;
                user.Hash = Convert.ToBase64String(salt);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    mensagem = "Sucesso"
                });
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }

        [HttpPost("salvarSintomas/{id}")]
        [Authorize]
        public async Task<IActionResult> SalvarSintomas([FromRoute] int id, [FromBody] SintomaViewModel model)
        {
            // Validar o ID do usuário no token JWT

            var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    return BadRequest(new
                    {
                        error = "Usuário não encontrado!"
                    });
                }

            // Criar o objeto Sintoma
            var sintoma = new Sintoma
            {
                UsuarioId = id,
                SintomaDescricao = model.sintoma,
                Dias = model.dias,
            };

            // Adicionar ao banco de dados
            _context.Sintomas.Add(sintoma);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Sintomas salvos com sucesso!" });
        }

        [HttpGet("pegarSintomas/{id}")]
        [Authorize]
        public async Task<IActionResult> pegarSintomas([FromRoute] int id)
        {
            // Validar o ID do usuário no token JWT

            var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return BadRequest(new
                {
                    error = "Usuário não encontrado!"
                });
            }

            // Criar o objeto Sintoma
            var listSintomas = await _context.Sintomas.Where(x => x.UsuarioId == user.Id).ToListAsync();

            return Ok(listSintomas);
        }
    }
}
