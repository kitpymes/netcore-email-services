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
            EmailAddressModel fromEmail,
            string subject,
            EmailBodyModel body,
            List<EmailAddressModel> to)
             : base(username, password, host, port, enableSsl, fromEmail, subject, body, to)
        {
        }

        public static IEmailConfiguration CreateDefault()
        {
            // REEMPLAZAR.
            var username = "hola@obelisco.shop";
            var password = "Shop01";
            var host = "smtpout.europe.secureserver.net";
            var port = 25;
            var enableSsl = false;
            var fromEmail = new EmailAddressModel("hola@obelisco.shop", "");
            var subject = "Email Test .NET";
            var to = new List<EmailAddressModel> {
                new EmailAddressModel("sferrari.net@gmail.com", "")
            };

            var body = EmailBodyModel.CreateBodyHtmlTemplate(
                bodyHtmlTemplatePath: Directory.GetCurrentDirectory().Split("bin")[0] + @"Common\Files\Templates", 
                bodyTemplateName: EmailBodyModel.BodyTemplateName.Vencimiento,
                bodyHtmlTemplateValues: new Dictionary<string, string> {
                    { "{FECHA}", $"{DateTime.Now.Day} / {DateTime.Now.Month} / {DateTime.Now.Year}"},
                    { "{NOMBRE}", "Alberto" },
                    { "{CONTENIDO}", "<h1>Email Test</h1> <p>Este email fue enviado por " +
                          "<a href='https://sferrari.micv.online'>Analista Programador .NET FREELANCE</a> " +
                         "<b>Utilizando .NET System.Net.Mail</b></p>"}
                });

            return new EmailNetConfiguration(username, password, host, port, enableSsl, fromEmail, subject, body, to)
            {
                UseDefaultCredentials = false,
                EmailExchange = null, // new EmailExchangeModel(ExchangeVersion.Exchange2007_SP1, TraceFlags.All),
                Timeout = 6000,
                Cc = null,
                Bcc = null,
                Attachement =  new EmailAttachementModel(Directory.GetCurrentDirectory().Split("bin")[0] + @"Common\Files\Attachments"),
                Zip = new EmailZipModel(Utils.CreateAndGetDirectoryTemporary(true))
            };
        }
    }
}