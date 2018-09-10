namespace Email
{
    public class EmailAttachementModel
    {
        public EmailAttachementModel(string attachementPathDirectory)
        {
            if (string.IsNullOrEmpty(attachementPathDirectory))
            {
                throw new System.ApplicationException($"El parametro {nameof(attachementPathDirectory)}, es requerido.");
            }

            AttachementPathDirectory = attachementPathDirectory;
        }

        public string AttachementPathDirectory { get; }
        public bool IsAttachement { get { return !string.IsNullOrEmpty(AttachementPathDirectory); } }
    }
}
