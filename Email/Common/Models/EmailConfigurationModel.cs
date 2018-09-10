using System.Collections.Generic;
using System.Linq;

namespace Email
{
    public abstract class EmailConfigurationModel : IEmailConfiguration
    {
        protected EmailConfigurationModel(
           string username,
           string password,
           string host,
           int port,
           bool enableSsl,
           EmailAddressModel emailAddress,
           string subject,
           string body,
           List<EmailAddressModel> to)
        {
            if (string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(host)
                || string.IsNullOrEmpty(subject)
                || string.IsNullOrEmpty(body)
                || to?.Any() != true)
            {
                throw new System.ApplicationException($"Los parámetros " +
                    $"{nameof(username)}, " +
                    $"{nameof(password)}," +
                    $"{nameof(host)}," +
                    $"{nameof(subject)}," +
                    $"{nameof(body)}," +
                    $"{nameof(to)} " +
                    $"son requeridos.");
            }

            Username = username;
            Password = password;
            Host = host;
            Port = port;
            EnableSsl = enableSsl;
            EmailAddress = emailAddress;
            Subject = subject;
            Body = body;
            To = to;
        }
        
        // Obligatorios
        public string Username { get; protected set; }
        public string Password { get; protected set; }
        public string Host { get; protected set; }
        public int Port { get; protected set; }
        public bool EnableSsl { get; protected set; }
        public EmailAddressModel EmailAddress { get; protected set; }
        public string Subject { get; protected set; }
        public string Body { get; protected set; }
        public List<EmailAddressModel> To { get; protected set; }

        // Opcionales
        public int Timeout { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public List<EmailAddressModel> Cc { get; set; }
        public List<EmailAddressModel> Bcc { get; set; }
        public EmailZipModel Zip { get; set; }
        public EmailAttachementModel Attachement { get; set; }
    }
}