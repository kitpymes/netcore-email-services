using System.Collections.Generic;
using System.IO;

namespace Email
{
    public class SendGridEmailConfiguration : EmailConfigurationModel
    {
        public string ApyKey { get; }

        public SendGridEmailConfiguration(
           string apyKey,
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
            if (string.IsNullOrEmpty(apyKey))
            {
                throw new System.ApplicationException($"El parámetro {nameof(apyKey)} es requerido.");
            }

            ApyKey = apyKey;
        }

        public static IEmailConfiguration CreateDefault()
        {
            // Parámetros obligatorios, reemplazar.
            var apyKey = "[APIKEY]";
            var username = "[USUARIO]";
            var password = "[CONTRASEÑA]";
            var host = "smtp.sendgrid.net";
            var port = 25;
            var enableSsl = true;
            var fromEmail = new EmailAddressModel("[DE EMAIL]", "");
            var subject = "Email Test Send Grid";

            var body = "<h1>Email Test</h1> <p>Este email fue enviado por " +
                      "<a href='https://sferrari.micv.online'>Analista Programador .NET FREELANCE</a> " +
                      "<b>Utilizando SendGrid</b></p>";

            var to = new List<EmailAddressModel> {
                new EmailAddressModel("[PARA EMAIL]", "")
            };

            return new SendGridEmailConfiguration(apyKey, username, password, host, port, enableSsl, fromEmail, subject, body, to)
            {
                UseDefaultCredentials = false,
                Timeout = 6000,
                Cc = null,
                Bcc = null,
                Attachement = new EmailAttachementModel(Directory.GetCurrentDirectory().Split("bin")[0] + @"Common\Files\Attachments"),
                Zip = new EmailZipModel(Utils.CreateAndGetDirectoryTemporary(true))
            };
        }
    }
}
