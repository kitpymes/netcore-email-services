using System.Collections.Generic;

namespace Email
{
    public interface IEmailConfiguration
    {
        // Obligatorios
        string Username { get; }
        string Password { get; }
        string Host { get; }
        int Port { get; }
        bool EnableSsl { get; }
        EmailAddressModel EmailAddress { get; }
        string Subject { get; }
        string Body { get; }
        List<EmailAddressModel> To { get; }

        // Opcionales
        int Timeout { get; }
        bool UseDefaultCredentials { get; }
        List<EmailAddressModel> Cc { get; }
        List<EmailAddressModel> Bcc { get; }
        EmailZipModel Zip { get; }
        EmailAttachementModel Attachement { get; }
    }
}
