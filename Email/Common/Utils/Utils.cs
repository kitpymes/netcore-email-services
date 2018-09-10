using System;
using System.Collections.Generic;
using System.IO;

namespace Email
{
    public static class Utils
    {
        public static List<(string filename, string filePath, string fileConvert, byte[] fileBytes)> ReadAllBytes(string directoryPath, string extensions = "*.*")
        {
                if (!Directory.Exists(directoryPath))
            {
                throw new ApplicationException($"El directorio {directoryPath} no existe.");
            }

            var directorySelected = Directory.GetFiles(directoryPath, extensions);

            List<(string filename, string filePath, string fileConvert, byte[] fileBytes)> filesReading = new List<(string filename, string filePath, string fileConvert, byte[] fileBytes)>();

            foreach (var filePath in directorySelected)
            {
                if (!File.Exists(filePath))
                {
                    throw new ApplicationException($"El archivo {filePath} no existe.");
                }

                var fileBytes = File.ReadAllBytes(filePath);
                var fileConvert = Convert.ToBase64String(fileBytes);
                var filename = filePath.Substring(filePath.LastIndexOf(@"\") + 1);

                filesReading.Add((filename: filename, filePath: filePath, fileConvert: fileConvert, fileBytes: fileBytes));
            }

            return filesReading;
        }

        public static string CreateAndGetDirectoryTemporary(bool withRandomFolder = true)
        {
            string tempDirectory = withRandomFolder ? Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()) : Path.GetTempPath();
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        public static void DeleteFilesDirectory(string pathDirectory, bool deleteDirectory)
        {
            foreach (var file in Directory.GetFiles(pathDirectory))
            {
                var attr = File.GetAttributes(file);
                if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(file, attr ^ FileAttributes.ReadOnly);
                }

                File.Delete(file);
            }

            if (deleteDirectory)
            {
                Directory.Delete(pathDirectory);
            }
        }
    }
}
