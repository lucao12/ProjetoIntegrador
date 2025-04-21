using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ProjetoIntegrador.Models;
using ProjetoIntegrador.ViewModel;
using System.Net;
using System.Net.Mime;
using System.Text.RegularExpressions;
using ProjetoIntegrador.Data;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using ProjetoIntegrador.Services;

namespace ProjetoIntegrador.Controllers
{
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmailController(AppDbContext context)
        {
            _context = context;
        }

        //Futuramente mover as credenciais de risco como senha para variaveis de ambiente seguras.

        [Authorize]
        [HttpPost]
        [Route("/EnviaEmail")]
        public async Task<IActionResult> EnviaEmails(
            [FromBody] EmailViewModelEnviar model)
        {
            try
            {
                var usuario = await _context.Usuarios
                   .FirstOrDefaultAsync(u => u.Id == model.idusuario);

                if (usuario == null)
                {
                    return NotFound(new { error = "Usuário não encontrado2" });
                }

                SmtpClient smtpClient = new SmtpClient("smtp.Office365.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("ls179841@alunos.unisanta.br", "$$Chewie290696@");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("ls179841@alunos.unisanta.br");
                mailMessage.To.Add(new MailAddress(usuario.Email));
                mailMessage.Subject = model.titulo;
                mailMessage.Body = model.body;

                smtpClient.Send(mailMessage);
                return Ok(model.body);
            }
            catch
            {
                return StatusCode(500, new { error = "Erro interno do servidor!" });
            }
        }
        [Authorize]
        [HttpPost]
        [Route("/EnviaEmailAnexo")]
        public async Task<IActionResult> EnviaEmailsAnexo([FromBody] EmailViewModelEnviarAnexo model)
        {
            try
            {
                var usuario = await _context.Usuarios
                   .FirstOrDefaultAsync(u => u.Id == model.idusuario);

                if (usuario == null)
                {
                    return NotFound(new { error = "Usuário não encontrado2" });
                }

                // Converte a string base64 para um array de bytes
                var base64Data = Regex.Match(model.anexoBase64, @"base64,(.*)").Groups[1].Value;
                byte[] fileBytes = Convert.FromBase64String(base64Data);

                // Cria um stream do conteúdo do anexo
                using var memoryStream = new MemoryStream(fileBytes);
                memoryStream.Position = 0;

                // Cria o anexo
                Attachment attachment = new Attachment(memoryStream, model.nomeArquivo, MediaTypeNames.Application.Pdf);

                // Configuração do cliente SMTP
                SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("ls179841@alunos.unisanta.br", "$$Chewie290696@") // (ideal: mova isso para appsettings.json)
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("ls179841@alunos.unisanta.br"),
                    Subject = model.titulo,
                    Body = model.body,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(new MailAddress(usuario.Email));
                mailMessage.Attachments.Add(attachment);

                await smtpClient.SendMailAsync(mailMessage);

                return Ok(new { mensagem = "E-mail com anexo enviado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno do servidor!", detalhes = ex.Message });
            }
        }
        [Authorize]
        [HttpPost]
        [Route("/AgendarEmail")]
        public async Task<IActionResult> AgendarEmail(
        [FromBody] EmailViewModelEnviar model,
        [FromServices] EmailServices emailService)
        {
            try
            {
                var usuario = await _context.Usuarios
                   .FirstOrDefaultAsync(u => u.Id == model.idusuario);

                if (usuario == null)
                    return NotFound(new { error = "Usuário não encontrado" });

                BackgroundJob.Schedule(
                    () => emailService.EnviarEmailAsync(usuario.Email, model.titulo, model.body),
                    TimeSpan.FromDays(7)); // 1 semana

                return Ok("Email agendado com sucesso para daqui 1 semana.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno do servidor", detalhes = ex.Message });
            }
        }

    }
}
