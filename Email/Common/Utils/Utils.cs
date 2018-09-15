using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Email
{
    public static class Utils
    {
        #region Helpers

        public static List<(string filename, string filePath, string fileConvert, byte[] fileBytes)> ReadAllBytes(string directoryPath, string extensions = "*.*")
        {
            IsDirectoryExistsThrowException(directoryPath);

            var directorySelected = Directory.GetFiles(directoryPath, extensions);

            List<(string filename, string filePath, string fileConvert, byte[] fileBytes)> filesReading = new List<(string filename, string filePath, string fileConvert, byte[] fileBytes)>();

            foreach (var filePath in directorySelected)
            {
                IsFileExistsThrowException(filePath);

                var fileBytes = File.ReadAllBytes(filePath);
                var fileConvert = Convert.ToBase64String(fileBytes);
                var filename = filePath.Substring(filePath.LastIndexOf(@"\") + 1);

                filesReading.Add((filename: filename, filePath: filePath, fileConvert: fileConvert, fileBytes: fileBytes));
            }

            return filesReading;
        }

        public static string GetDirectoryFromProject(string directoryPath)
        {
            var path = Directory.GetCurrentDirectory().Split("bin")[0] + directoryPath;

            IsDirectoryExistsThrowException(path);

            return path;
        }

        public static string CreateAndGetDirectoryTemporary(bool withRandomFolder = true)
        {
            string tempDirectory = withRandomFolder ? Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()) : Path.GetTempPath();

            try
            {
                Directory.CreateDirectory(tempDirectory);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return tempDirectory;
        }

        public static void DeleteFilesDirectory(string pathDirectory, bool deleteDirectory)
        {
            IsDirectoryExistsThrowException(pathDirectory);

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

        public static void ZipFiles(string filesToZipPathDirectory, string saveZipPathDirectory, bool removeSaveZipPathDirectory, string zipName = "/files.zip")
        {
            try
            {
                IsDirectoryExistsThrowException(filesToZipPathDirectory);
                IsDirectoryExistsThrowException(saveZipPathDirectory);

                ZipFile.CreateFromDirectory(filesToZipPathDirectory, saveZipPathDirectory + zipName);
            }
            catch (UnauthorizedAccessException ex)
            {
                if (removeSaveZipPathDirectory)
                {
                    Utils.DeleteFilesDirectory(saveZipPathDirectory, true);
                }

                throw new ApplicationException(ex.Message);
            }
        }

        public static void Show(string message, bool startLineSpace = false, bool lastLineSpace = false, bool readLine = false)
        {
            if (startLineSpace)
            {
                Console.WriteLine();
            }
      
            Console.WriteLine(message);

            if (lastLineSpace)
            {
                Console.WriteLine();
            }

            if (readLine)
            {
                Console.ReadLine();
            }
        }

        #endregion Helpers

        #region Validations

        public static void IsDirectoryExistsThrowException(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new ApplicationException($"El directorio {directoryPath} no existe.");
            }
        }

        public static void IsFileExistsThrowException(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ApplicationException($"El archivo {filePath} no existe.");
            }
        }

        public static string IsNullOrEmptyThrowException(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ApplicationException($"El parámetro {name}, es requerido.");
            }

            return value;
        }

        public static T IsNullThrowException<T>(object value, string name)
        {
            if (value is null)
            {
                throw new ApplicationException($"El parámetro {name}, es requerido.");
            }

            return (T)value;
        }

        public static List<T> IsNullAndCountThrowException<T>(List<T> value, string name)
        {
            if (value?.Any() != true)
            {
                throw new ApplicationException($"El parámetro {name}, es requerido.");
            }

            return value;
        }

        public static TResult IsEqualTypeThrowException<TSource, TResult>(TSource value, string name)
        {
            TResult result = default;

            if (!(value is TResult))
            {
                throw new ApplicationException($"El parámeto {name} debe ser de tipo {result.GetType()}, pero es de tipo {value.GetType()}");
            }

            return (TResult)Convert.ChangeType(value, typeof(TResult));
        }

        #endregion Validations
    }
}
