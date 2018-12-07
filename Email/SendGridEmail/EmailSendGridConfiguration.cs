using System;
using System.Collections.Generic;

namespace Email
{
    public class EmailSendGridConfiguration : EmailConfigurationModel
    {
        public string ApyKey { get; }

        public EmailSendGridConfiguration(
           string apyKey,
           string username,
            string password,
            EmailAddressModel fromEmail,
            List<EmailAddressModel> toEmail,
            string host,
            int port,
            bool enableSsl,
            string subject,
            EmailBodyModel body)
             : base(username, password, fromEmail, toEmail, host, port, enableSsl, subject, body)
        {
            ApyKey = Utils.IsNullOrEmptyThrowException(apyKey, nameof(apyKey));
        }

        public static IEmailConfiguration CreateDefault()
        {
            // REEMPLAZAR CAMPOS OBLIGATORIOS
            return new EmailSendGridConfiguration(
                apyKey: "",
                username: "",
                password: "",
                fromEmail: new EmailAddressModel("", ""),
                toEmail: new List<EmailAddressModel> {
                    new EmailAddressModel("", "")
                },
                host: "smtp.gmail.com",
                port: 587,
                enableSsl: true,
                subject: "Email Test .NET",
                body: EmailBodyModel.CreateBodyHtmlTemplate(
                    bodyHtmlTemplatePath: Utils.GetDirectoryFromProject(@"Common\Files\Templates"),
                    bodyTemplateName: EmailBodyModel.BodyTemplateName.Vencimiento,
                    bodyHtmlTemplateValues: new Dictionary<string, string> {
                        { "{FECHA}", $"{DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year}"},
                        { "{NOMBRE}", "Alberto" },
                        { "{CONTENIDO}", "<h1>Email Test</h1> <p>Este email fue enviado por " +
                              "<a href='https://sferrari.micv.pro' target='_blank'>Analista Programador .NET FREELANCE</a> " +
                             "<b>Utilizando SendGrid</b></p>"}
                    }))
            {
                // REEMPLAZAR CAMPOS OPCIONALES
                UseDefaultCredentials = false,
                Timeout = 6000,
                Cc = null,
                Bcc = null,
                Attachement = new EmailAttachementModel(Utils.GetDirectoryFromProject(@"Common\Files\Attachments")),
                Zip = new EmailZipModel(Utils.CreateAndGetDirectoryTemporary(true))
            };
        }
    }
}
