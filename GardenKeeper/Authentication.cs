using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GardenKeeper
{
    internal static class Authentication
    {
        static List<Users> users = Core.context.Users.ToList();

        public static bool IsAuthenticated(string email, string password)
        {
            MD5 hasher = MD5.Create();

            byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            foreach (var user in users)
            {
                if(user.Email == email && user.PasswordHash == sBuilder.ToString())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
