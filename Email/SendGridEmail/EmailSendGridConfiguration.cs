using System;
using System.Collections.Generic;
using System.IO;

namespace Email
{
    public class EmailSendGridConfiguration : EmailConfigurationModel
    {
        public string ApyKey { get; }

        public EmailSendGridConfiguration(
           string apyKey,
           string username,
           string password,
           string host,
           int port,
           bool enableSsl,
           EmailAddressModel fromEmail,
           string subject,
           EmailBodyModel body,
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
            // REEMPLAZAR.
            var apyKey = "[APIKEY]";
            var username = "[USUARIO]";
            var password = "[CONTRASEÑA]";
            var host = "smtp.sendgrid.net";
            var port = 25;
            var enableSsl = true;
            var fromEmail = new EmailAddressModel("[DE EMAIL]", "");
            var subject = "Email Test Send Grid";
            var to = new List<EmailAddressModel> {
                new EmailAddressModel("[PARA EMAIL]", "")
            };

            var body = EmailBodyModel.CreateBodyHtmlTemplate(
                bodyHtmlTemplatePath: Directory.GetCurrentDirectory().Split("bin")[0] + @"Common\Files\Templates",
                bodyTemplateName: EmailBodyModel.BodyTemplateName.Vencimiento,
                bodyHtmlTemplateValues: new Dictionary<string, string> {
                    { "{FECHA}", $"{DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year}"},
                    { "{NOMBRE}", "Alberto" },
                    { "{CONTENIDO}", "<h1>Email Test</h1> <p>Este email fue enviado por " +
                          "<a href='https://sferrari.micv.online'>Analista Programador .NET FREELANCE</a> " +
                         "<b>Utilizando SendGrid</b></p>"}
                });

            return new EmailSendGridConfiguration(apyKey, username, password, host, port, enableSsl, fromEmail, subject, body, to)
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
