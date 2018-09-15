using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Exchange.WebServices.Data;

namespace Email
{
    public class EmailNetConfiguration : EmailConfigurationModel
    {
        public EmailExchangeModel EmailExchange { get; set; }
        public bool IsExchange { get { return EmailExchange != null; } }

        public EmailNetConfiguration(
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
        }

        public static IEmailConfiguration CreateDefault()
        {
            // REEMPLAZAR.
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
                bodyHtmlTemplatePath: Utils.GetDirectoryFromProject(@"Common\Files\Templates"), 
                bodyTemplateName: EmailBodyModel.BodyTemplateName.Vencimiento,
                bodyHtmlTemplateValues: new Dictionary<string, string> {
                    { "{FECHA}", $"{DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year}"},
                    { "{NOMBRE}", "Alberto" },
                    { "{CONTENIDO}", "<h1>Email Test</h1> <p>Este email fue enviado por " +
                          "<a href='https://sferrari.micv.online'>Analista Programador .NET FREELANCE</a> " +
                         "<b>Utilizando .NET System.Net.Mail</b></p>"}
                });

            return new EmailNetConfiguration(username, password, host, port, enableSsl, subject, fromEmail, body, to)
            {
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