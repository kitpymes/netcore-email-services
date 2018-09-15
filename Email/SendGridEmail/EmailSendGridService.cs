using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Email
{
    public class EmailSendGridService : IEmailService
    {
        public async Task<bool> SendAsync(IEmailConfiguration config)
        {
            Utils.IsNullThrowException<IEmailConfiguration>(config, nameof(config));

            var emailConfig = Utils.IsEqualTypeThrowException<IEmailConfiguration, EmailSendGridConfiguration>(config, nameof(config));

            var client = new SendGridClient(emailConfig.ApyKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(emailConfig.EmailAddress.FromEmail, emailConfig.EmailAddress.FromName),
                Subject = emailConfig.Subject,
                HtmlContent = emailConfig.Body.Value
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
                Utils.IsDirectoryExistsThrowException(emailConfig.Attachement.AttachementPathDirectory);

                if (emailConfig.Zip.IsCompressed)
                {
                    Utils.ZipFiles(emailConfig.Attachement.AttachementPathDirectory, emailConfig.Zip.ZipPathDirectory, emailConfig.Zip.IsDelete);

                    var file = Utils.ReadAllBytes(emailConfig.Zip.ZipPathDirectory).FirstOrDefault();

                    msg.AddAttachment(file.filename, file.fileConvert);

                    if (emailConfig.Zip.IsDelete)
                    {
                        Utils.DeleteFilesDirectory(emailConfig.Zip.ZipPathDirectory, true);
                    }
                }
                else
                {
                    foreach (var file in Utils.ReadAllBytes(emailConfig.Attachement.AttachementPathDirectory))
                    {
                        msg.AddAttachment(file.filename, file.fileConvert);
                    }
                }
            }

            Utils.Show($"Enviando email con {nameof(EmailSendGridService)}... Espere por favor...", true);

            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            var message = ((response.StatusCode == System.Net.HttpStatusCode.Accepted)
                ? "El email fue enviado correctamente! "
                : "El email NO pudo ser enviado! ") + $"El StatusCode devuelto por SendGrid es: { response.StatusCode}";

            Utils.Show(message);

            return response.StatusCode == System.Net.HttpStatusCode.Accepted;

        }
    }
}
