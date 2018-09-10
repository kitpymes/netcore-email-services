using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Exchange = Microsoft.Exchange.WebServices.Data;

namespace Email
{
    public class NetEmailService : IEmailService
    {
        public async Task<bool> SendAsync(IEmailConfiguration config)
        {
            if (config is null)
            {
                throw new ApplicationException($"{nameof(config)} es null.");
            }

            if (!(config is NetEmailConfiguration))
            {
                throw new ApplicationException($"{nameof(config)} es de tipo {config.GetType()} , pero tiene que ser del tipo {nameof(NetEmailConfiguration)}");
            }

            var emailConfig = config as NetEmailConfiguration;

            if (emailConfig.IsExchange)
            {
                Exchange.ExchangeService service = new Exchange.ExchangeService(emailConfig.EmailExchange.ExchangeVersion);
                service.Credentials = new NetworkCredential(emailConfig.Username, emailConfig.Password);

                if (emailConfig.EmailExchange.IsTraceEnabled)
                {
                    service.TraceEnabled = true;
                    service.TraceFlags = emailConfig.EmailExchange.TraceFlags;
                }
               
                service.AutodiscoverUrl(emailConfig.EmailAddress.FromEmail, RedirectionUrlValidationCallback);

                Exchange.EmailMessage message = new Exchange.EmailMessage(service);
                message.Subject = emailConfig.Subject;
                message.Body = new Exchange.MessageBody(emailConfig.Body);
                emailConfig.To.ForEach(to => message.ToRecipients.Add(new Exchange.EmailAddress(to.FromEmail)));

                if (emailConfig.Bcc?.Count > 0)
                {
                    emailConfig.Bcc.ForEach(bcc => message.BccRecipients.Add(new Exchange.EmailAddress(bcc.FromEmail)));
                }

                if (emailConfig.Cc?.Count > 0)
                {
                    emailConfig.Cc.ForEach(cc => message.CcRecipients.Add(new Exchange.EmailAddress(cc.FromEmail)));
                }

                if (emailConfig.Attachement.IsAttachement)
                {
                    if (!Directory.Exists(emailConfig.Attachement.AttachementPathDirectory))
                    {
                        throw new ApplicationException($"El directorio {emailConfig.Attachement.AttachementPathDirectory} no existe.");
                    }

                    if (emailConfig.Zip.IsCompressed)
                    {
                        CompressedFiles(emailConfig);

                        var file = Utils.ReadAllBytes(emailConfig.Zip.ZipPathDirectory).FirstOrDefault();

                        message.Attachments.AddFileAttachment(file.filename);

                        if (emailConfig.Zip.IsDelete)
                        {
                            Utils.DeleteFilesDirectory(emailConfig.Zip.ZipPathDirectory, true);
                        }
                    }
                    else
                    {
                        foreach (var file in Utils.ReadAllBytes(emailConfig.Attachement.AttachementPathDirectory))
                        {
                            message.Attachments.AddFileAttachment(file.filename);
                        }
                    }
                }

                Console.WriteLine($"\nEnviando email con {nameof(NetEmailService)}...");

                message.Save();
                message.SendAndSaveCopy();
            }
            else
            {
                var msg = new MailMessage{
                    IsBodyHtml = true,
                    From = new MailAddress(emailConfig.EmailAddress.FromEmail, emailConfig.EmailAddress.FromName),
                    Subject = emailConfig.Subject,
                    Body = emailConfig.Body
                };

                emailConfig.To.ForEach(to => msg.To.Add(new MailAddress(to.FromEmail, to.FromName)));

                if (emailConfig.Bcc?.Count > 0)
                {
                    emailConfig.Bcc.ForEach(bcc => msg.Bcc.Add(new MailAddress(bcc.FromEmail, bcc.FromName)));
                }

                if (emailConfig.Cc?.Count > 0)
                {
                    emailConfig.Cc.ForEach(cc => msg.CC.Add(new MailAddress(cc.FromEmail, cc.FromName)));
                }

                if (emailConfig.Attachement.IsAttachement)
                {
                    if (!Directory.Exists(emailConfig.Attachement.AttachementPathDirectory))
                    {
                        throw new ApplicationException($"El directorio {emailConfig.Attachement.AttachementPathDirectory} no existe.");
                    }

                    if (emailConfig.Zip.IsCompressed)
                    {
                        CompressedFiles(emailConfig);

                        var file = Utils.ReadAllBytes(emailConfig.Zip.ZipPathDirectory).FirstOrDefault();

                        msg.Attachments.Add(new Attachment(new MemoryStream(file.fileBytes), file.filename));

                        if (emailConfig.Zip.IsDelete)
                        {
                            Utils.DeleteFilesDirectory(emailConfig.Zip.ZipPathDirectory, true);
                        }

                    }
                    else
                    {
                        foreach (var file in Utils.ReadAllBytes(emailConfig.Attachement.AttachementPathDirectory))
                        {
                            msg.Attachments.Add(new Attachment(new MemoryStream(file.fileBytes), file.filename));
                        }
                    }
                }

                using (var client = new SmtpClient(emailConfig.Host, emailConfig.Port))
                {
                    client.Credentials = new NetworkCredential(emailConfig.Username, emailConfig.Password);
                    client.EnableSsl = emailConfig.EnableSsl;

                    client.SendCompleted += delegate (object sender, System.ComponentModel.AsyncCompletedEventArgs e)
                    {
                        if (e.Error != null)
                        {
                            Console.WriteLine($"El email NO pudo ser enviado ! Ocurio el siguiente error: {e.Error.Message}");
                        }
                        else
                        {
                            Console.WriteLine($"El email fue enviado correctamente !");
                        }

                        var userMessage = e.UserState as MailMessage;
                        if (userMessage != null)
                        {
                            userMessage.Dispose();
                        }
                    };

                    Console.WriteLine($"\nEnviando email con {nameof(NetEmailService)}...");

                    await client.SendMailAsync(msg);
                }
            }

            return await Task.FromResult(true);
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }

        private static void CompressedFiles(NetEmailConfiguration emailConfig)
        {
            try
            {
                ZipFile.CreateFromDirectory(emailConfig.Attachement.AttachementPathDirectory, emailConfig.Zip.ZipPathDirectory + "/files.zip");
            }
            catch (UnauthorizedAccessException ex)
            {
                if (emailConfig.Zip.IsDelete)
                {
                    Utils.DeleteFilesDirectory(emailConfig.Zip.ZipPathDirectory, true);
                }

                throw;
            }
        }
    }
}
