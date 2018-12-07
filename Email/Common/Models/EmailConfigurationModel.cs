using System.Collections.Generic;

namespace Email
{
    public abstract class EmailConfigurationModel : IEmailConfiguration
    {
        protected EmailConfigurationModel(
           string username,
           string password,
           EmailAddressModel fromEmail,
           List<EmailAddressModel> toEmail,
           string host,
           int port,
           bool enableSsl,
           string subject,
           EmailBodyModel body)
        {
            Username = Utils.IsNullOrEmptyThrowException(username, nameof(username));
            Password = Utils.IsNullOrEmptyThrowException(password, nameof(password));
            EmailAddress = Utils.IsNullThrowException<EmailAddressModel>(fromEmail, nameof(fromEmail));
            To = Utils.IsNullAndCountThrowException(toEmail, nameof(toEmail));
            Host = Utils.IsNullOrEmptyThrowException(host, nameof(host));
            Port = port;
            EnableSsl = enableSsl;
            Subject = Utils.IsNullOrEmptyThrowException(subject, nameof(subject));
            Body = Utils.IsNullThrowException<EmailBodyModel>(body, nameof(body));
        }
        
        // Obligatorios
        public string Username { get; protected set; }
        public string Password { get; protected set; }
        public string Host { get; protected set; }
        public int Port { get; protected set; }
        public bool EnableSsl { get; protected set; }
        public string Subject { get; protected set; }
        public EmailAddressModel EmailAddress { get; protected set; }
        public EmailBodyModel Body { get; protected set; }
        public List<EmailAddressModel> To { get; protected set; }

        // Opcionales
        public int Timeout { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public EmailZipModel Zip { get; set; }
        public EmailAttachementModel Attachement { get; set; }
        public List<EmailAddressModel> Cc { get; set; }
        public List<EmailAddressModel> Bcc { get; set; }
    }
}