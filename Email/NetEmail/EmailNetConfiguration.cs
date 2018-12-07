using System;
using System.Collections.Generic;

namespace Email
{
    public class EmailNetConfiguration : EmailConfigurationModel
    {
        public EmailExchangeModel EmailExchange { get; set; }
        public bool IsExchange => EmailExchange != null;

        public EmailNetConfiguration(
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
        }

        public static IEmailConfiguration CreateDefault()
        {
            // REEMPLAZAR CAMPOS OBLIGATORIOS
            return new EmailNetConfiguration(
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
                             "<b>Utilizando .NET System.Net.Mail</b></p>"}
                    }))
            {
                // REEMPLAZAR CAMPOS OPCIONALES
                UseDefaultCredentials = false,
                EmailExchange = null, // new EmailExchangeModel(ExchangeVersion.Exchange2007_SP1, TraceFlags.All),
                Timeout = 6000,
                Cc = null,
                Bcc = null,
                Attachement =  new EmailAttachementModel(Utils.GetDirectoryFromProject(@"Common\Files\Attachments")),
                Zip = new EmailZipModel(Utils.CreateAndGetDirectoryTemporary(true))
            };
        }
    }
}