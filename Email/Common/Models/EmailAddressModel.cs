namespace Email
{
    public class EmailAddressModel
    {
        public EmailAddressModel(string fromEmail, string fromName = "")
        {
            if (string.IsNullOrEmpty(fromEmail))
            {
                throw new System.ApplicationException($"El parametro {nameof(fromEmail)}, es requerido.");
            }

            FromEmail = fromEmail;
            FromName = fromName; 
        }

        public string FromEmail { get;}
        public string FromName { get; }
    }
}
