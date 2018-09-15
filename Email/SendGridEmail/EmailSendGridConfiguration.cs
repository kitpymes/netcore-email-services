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
           string subject,
           EmailAddressModel fromEmail,
           EmailBodyModel body,
           List<EmailAddressModel> to)
            : base(username, password, host, port, enableSsl, subject, fromEmail, body, to)
        {
            ApyKey = Utils.IsNullOrEmptyThrowException(apyKey, nameof(apyKey));
        }

        public static IEmailConfiguration CreateDefault()
        {
            // REEMPLAZAR.
            var apyKey = "";
            var username = "";
            var password = "";
            var host = "";
            var port = 25;
            var enableSsl = false;
            var subject = "Email Test .NET";
            var fromEmail = new EmailAddressModel("", "");
            var to = new List<EmailAddressModel> {
                new EmailAddressModel("", "")
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

            return new EmailSendGridConfiguration(apyKey, username, password, host, port, enableSsl, subject, fromEmail, body, to)
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
