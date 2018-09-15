namespace Email
{
    public class EmailAttachementModel
    {
        public EmailAttachementModel(string attachementPathDirectory)
        {
            AttachementPathDirectory = Utils.IsNullOrEmptyThrowException(attachementPathDirectory, nameof(attachementPathDirectory));
        }

        public string AttachementPathDirectory { get; }
        public bool IsAttachement { get { return !string.IsNullOrEmpty(AttachementPathDirectory); } }
    }
}
