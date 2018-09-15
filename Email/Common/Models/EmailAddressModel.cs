namespace Email
{
    public class EmailAddressModel
    {
        public EmailAddressModel(string fromEmail, string fromName = "")
        {
            FromEmail = Utils.IsNullOrEmptyThrowException(fromEmail, nameof(fromEmail));
            FromName = fromName;
        }

        public string FromEmail { get;}
        public string FromName { get; }
    }
}
