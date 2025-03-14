using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GardenKeeper
{
    internal class EmailInteraction
    {
        static private string fromAddress = @"gardenkeeperofficial@gmail.com";
        static private string fromPassword = @"EPSjfqi5w94qq";
        static private bool isSent = false;

        public static void SendIvnite(string toAddress, string newPassword)
        {
            try
            {
                string body = $"Здравствуйте! \r\nВы запросили сброс пароля" +
                    $" в приложении GardenKeeper. Ваш новый пароль - {newPassword}" +
                    $"\r\n\r\n\r\nС уважением, {fromAddress}\r\n\r\n";
                string subject = $"Сброс пароля";

                MailAddress from = new MailAddress(fromAddress, "");
                MailAddress to = new MailAddress(toAddress);
                MailMessage message = new MailMessage(from, to);
                message.Subject = subject;
                message.Body = body;
                SmtpClient smtp = new SmtpClient("smtp.gmail.ru", 587);
                smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                smtp.EnableSsl = true;
                smtp.Send(message);
                isSent = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
