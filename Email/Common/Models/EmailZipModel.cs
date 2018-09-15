namespace Email
{
    public class EmailZipModel
    {
        public EmailZipModel(string zipPathDirectory, bool isDelete= true)
        {
            Utils.IsDirectoryExistsThrowException(zipPathDirectory);

            ZipPathDirectory = zipPathDirectory;
            IsDelete = isDelete;
        }

        public string ZipPathDirectory { get; }
        public bool IsCompressed { get {  return !string.IsNullOrEmpty(ZipPathDirectory); } }
        public bool IsDelete { get; }
    }
}
