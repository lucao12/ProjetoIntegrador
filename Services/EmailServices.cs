using System.Net.Mail;
using System.Net;

namespace ProjetoIntegrador.Services
{
    public class EmailServices
    {
        public async Task EnviarEmailAsync(string destinatario, string titulo, string body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.Office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("ls179841@alunos.unisanta.br", "$$Chewie290696@")
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("ls179841@alunos.unisanta.br"),
                Subject = titulo,
                Body = body
            };
            mailMessage.To.Add(new MailAddress(destinatario));

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
