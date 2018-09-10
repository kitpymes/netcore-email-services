using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Email
{
    public class SendGridEmailService : IEmailService
    {
        public async Task<bool> SendAsync(IEmailConfiguration config)
        {
            if (config is null)
            {
                throw new ApplicationException($"{nameof(config)} es null.");
            }

            if (!(config is SendGridEmailConfiguration))
            {
                throw new ApplicationException($"{nameof(config)} es de tipo {config.GetType()} , pero tiene que ser del tipo {nameof(SendGridEmailConfiguration)}");
            }

            var emailConfig = config as SendGridEmailConfiguration;

            var client = new SendGridClient(emailConfig.ApyKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(emailConfig.EmailAddress.FromEmail, emailConfig.EmailAddress.FromName),
                Subject = emailConfig.Subject,
                HtmlContent = emailConfig.Body 
            };

            emailConfig.To.ForEach(to => msg.AddTo(to.FromEmail, to.FromName));

            if (emailConfig.Bcc?.Count > 0)
            {
                emailConfig.Bcc.ForEach(bcc=> msg.AddBcc(bcc.FromEmail, bcc.FromName));
            }

            if (emailConfig.Cc?.Count > 0)
            {
                emailConfig.Cc.ForEach(cc => msg.AddCc(cc.FromEmail, cc.FromName));
            }

            if (emailConfig.Attachement.IsAttachement)
            {
                if (!Directory.Exists(emailConfig.Attachement.AttachementPathDirectory))
                {
                    throw new ApplicationException($"El directorio {emailConfig.Attachement.AttachementPathDirectory} no existe.");
                }

                if (emailConfig.Zip.IsCompressed)
                {
                    CompressFiles(emailConfig, msg);
                }
                else
                {
                    foreach (var file in Utils.ReadAllBytes(emailConfig.Attachement.AttachementPathDirectory))
                    {
                        msg.AddAttachment(file.filename, file.fileConvert);
                    }
                }
            }

            Console.WriteLine($"\nEnviando email con {nameof(SendGridEmailService)}...");

            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            var message = (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                ? $"El email fue enviado correctamente! El StatusCode es: {response.StatusCode}"
                : $"El email NO pudo ser enviado! El StatusCode es: {response.StatusCode}";

            Console.WriteLine(message);

            return response.StatusCode == System.Net.HttpStatusCode.Accepted;

        }

        private static void CompressFiles(SendGridEmailConfiguration emailConfig, SendGridMessage msg)
        {
            try
            {
                ZipFile.CreateFromDirectory(emailConfig.Attachement.AttachementPathDirectory, emailConfig.Zip.ZipPathDirectory + "/files.zip");

                var file = Utils.ReadAllBytes(emailConfig.Zip.ZipPathDirectory).FirstOrDefault();

                msg.AddAttachment(file.filename, file.fileConvert);

                if (emailConfig.Zip.IsDelete)
                {
                    Utils.DeleteFilesDirectory(emailConfig.Zip.ZipPathDirectory, true);
                }

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
