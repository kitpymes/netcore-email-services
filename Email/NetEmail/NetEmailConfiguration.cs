using System.Collections.Generic;
using System.IO;
using Exchange = Microsoft.Exchange.WebServices.Data;

namespace Email
{
    public class NetEmailConfiguration : EmailConfigurationModel
    {
        public EmailExchangeModel EmailExchange { get; set; }
        public bool IsExchange { get { return EmailExchange != null; } }

        public NetEmailConfiguration(
            string username,
            string password,
            string host,
            int port,
            bool enableSsl,
            EmailAddressModel fromEmail,
            string subject,
            string body,
            List<EmailAddressModel> to)
             : base(username, password, host, port, enableSsl, fromEmail, subject, body, to)
        {
        }

        public static IEmailConfiguration CreateDefault()
        {
            // Parámetros obligatorios, reemplazar.
            var username = "[USUARIO]";
            var password = "[CONTRASEÑA]";
            var host = "[HOST]";
            var port = 25;
            var enableSsl = false;
            var fromEmail = new EmailAddressModel("[DE EMAIL]", "");
            var subject = "Email Test .NET";

            var body = "<h1>Email Test</h1> <p>Este email fue enviado por " +
                      "<a href='https://sferrari.micv.online'>Analista Programador .NET FREELANCE</a> " +
                     "<b>Utilizando .NET System.Net.Mail</b></p>";

            var to = new List<EmailAddressModel> {
                new EmailAddressModel("[PARA EMAIL]", "")
            };

            return new NetEmailConfiguration(username, password, host, port, enableSsl, fromEmail, subject, body, to)
            {
                UseDefaultCredentials = false,
                EmailExchange = new EmailExchangeModel(Exchange.ExchangeVersion.Exchange2007_SP1, Exchange.TraceFlags.All),
                Timeout = 6000,
                Cc = null,
                Bcc = null,
                Attachement =  new EmailAttachementModel(Directory.GetCurrentDirectory().Split("bin")[0] + @"Common\Files\Attachments"),
                Zip = new EmailZipModel(Utils.CreateAndGetDirectoryTemporary(true))
            };
        }
    }
}