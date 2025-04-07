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
        static private string fromAddress = @"garden.keeper@mail.ru";
        static private string fromPassword = @"m5fDQKF3wuuscg59Erah";
        static private bool isSent = false;
        /// <summary>
        /// Отправка письма для сброса пароля
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="newPassword"></param>
        public static void SendResetPassword(string toAddress, string newPassword)
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
                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
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
