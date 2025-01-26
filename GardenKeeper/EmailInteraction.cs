using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GardenKeeper
{
    internal class EmailInteraction
    {
        static private string fromAddress = @"";
        static private string fromPassword = @"";
        static public bool isSent = false;

        public static void SendIvnite(string toAddress, string str_code)
        {
            int code = int.Parse(str_code);
            try
            {
                string body = $"Здравствуйте! \r\nВы завершаете регистрацию" +
                    $" в приложении GardenKeeper. Ваш код подтверждения - {code}" +
                    $"\r\n\r\nПожалуйста, завершите верификацию учётной записи в приложении!" +
                    $"\r\n\r\n\r\nС уважением,\r\n\r\nTYom4ik";
                string subject = $"Ваш код подтверждения Work Flow - Task Tracker - {code}";

                MailAddress from = new MailAddress(fromAddress, "Work Flow - Task Tracker");
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
                Console.WriteLine(ex.Message);
            }
        }
    }
}
