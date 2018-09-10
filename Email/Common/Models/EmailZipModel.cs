namespace Email
{
    public class EmailZipModel
    {
        public EmailZipModel(string zipPathDirectory, bool isDelete= true)
        {
            if (string.IsNullOrEmpty(zipPathDirectory))
            {
                throw new System.ApplicationException($"El parametro {nameof(zipPathDirectory)}, es requerido.");
            }

            ZipPathDirectory = zipPathDirectory;
            IsDelete = isDelete;
        }

        public string ZipPathDirectory { get; }
        public bool IsCompressed { get {  return !string.IsNullOrEmpty(ZipPathDirectory); } }
        public bool IsDelete { get; }
    }
}
