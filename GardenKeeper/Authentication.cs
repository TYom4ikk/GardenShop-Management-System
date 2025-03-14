using GardenKeeper.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace GardenKeeper
{
    public static class Authentication
    {
        private static List<Users> users = Core.context.Users.ToList();
        public static Users IsAuthenticated(string email, string password)
        {
            MD5 hasher = MD5.Create();

            byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            StreamWriter sw = new StreamWriter("asd.txt", true);
            sw.WriteLine(sBuilder.ToString());
            sw.Close();
            foreach (var user in users)
            {
                if(user.Email == email && user.PasswordHash == sBuilder.ToString())
                {
                    return user;
                }
            }
            return null;
        }
    }
}
