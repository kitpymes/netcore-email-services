using System;

namespace Email
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Envia default email con .NET
                new EmailNetService().SendAsync(EmailNetConfiguration.CreateDefault()).Wait();

                // Envia custom email con .NET
                //new EmailNetService().SendAsync(new EmailNetConfiguration(
                //// Valores.....    
                //)).Wait();

                // Envia default email con SendGrid
                new EmailSendGridService().SendAsync(EmailSendGridConfiguration.CreateDefault()).Wait();

                // Envia custom email con SendGrid
                //new EmailSendGridService().SendAsync(new EmailSendGridConfiguration(
                ////Valores..... 
                //)).Wait();
            }
            catch (ApplicationException ex)
            {
                Utils.Show("Ocurrio el siguiente error:" + ex.Message, false, true);
            }
            catch (Exception ex)
            {
                Utils.Show("Ocurrio un error inesperado:" + ex.Message, false, true);
            }

            Utils.Show("Oprima una tecla para terminar...", true, readLine: true);
        }
    }
}
