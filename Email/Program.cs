using System;
using System.IO;

namespace Email
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Reemplazar la configuracion en la clase "EmailNetConfiguration"
                new EmailNetService().SendAsync(EmailNetConfiguration.CreateDefault()).Wait();

                // Reemplazar la configuracion en la clase "EmailSendGridConfiguration"
                //new EmailSendGridService().SendAsync(EmailSendGridConfiguration.CreateDefault()).Wait();
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine("Ocurrio el siguiente error:\n" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrio un error inesperado:\n" + ex.Message);
            }

            Console.WriteLine("\nOprima una tecla para continuar...");
            Console.ReadLine();
        }
    }
}
