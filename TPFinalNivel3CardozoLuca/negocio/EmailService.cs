using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace negocio
{
    public class EmailService
    {
        private MailMessage email;
        private SmtpClient server;

        // Cuenta emisora
        private string remitente = ConfigurationManager.AppSettings["Email"];
        private string password = ConfigurationManager.AppSettings["PasswordEmail"];

        public EmailService()
        {
            server = new SmtpClient();

            server.Host = "smtp.gmail.com";
            server.Port = 587;
            server.EnableSsl = true;
            server.DeliveryMethod = SmtpDeliveryMethod.Network;
            server.UseDefaultCredentials = false;

            server.Credentials = new NetworkCredential(remitente, password);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public void armarCorreo(string emailDestino, string asunto, string cuerpo)
        {
            email = new MailMessage();

            email.From = new MailAddress(remitente, "Catálogo Artículos Tecnológicos");
            email.To.Add(emailDestino);

            email.Subject = asunto;
            email.Body = cuerpo;
            email.IsBodyHtml = true;
        }

        public void enviarEmail()
        {
            try
            {
                server.Send(email);
            }
            catch (SmtpException ex)
            {
                throw new Exception(
                    $"SMTP ERROR\n" +
                    $"StatusCode: {ex.StatusCode}\n" +
                    $"Message: {ex.Message}\n" +
                    $"Inner: {ex.InnerException}"
                );
            }
        }
    }
}
