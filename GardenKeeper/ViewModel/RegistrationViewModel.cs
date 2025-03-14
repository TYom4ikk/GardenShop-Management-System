using GardenKeeper.Model;
using GardenKeeper.View.UsersView;
using System.Text;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Linq;

namespace GardenKeeper.ViewModel
{
    public class RegistrationViewModel
    {
        public Users Authenticate(string Login, string Password)
        {
            if (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password))
            {
                var currentUser = Authentication.IsAuthenticated(Login, Password);

                if (currentUser == null)
                {
                    return null;
                }
                return currentUser;
            }
            return null;
        }

        public string GeneratePassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            var random = new Random();
            var password = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                password.Append(validChars[random.Next(0, validChars.Length)]);
            }

            return password.ToString();
        }

        public string GenerateHash(string text)
        {
            MD5 hasher = MD5.Create();
            byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public void SaveUserRegistrationData(string email, string hash)
        {
            var user = Core.context.Users.Where(u=>u.Email == email).FirstOrDefault();
            user.PasswordHash = hash;
            Core.context.SaveChanges();
        }

        public bool IsUserExists(string email)
        {
            var user = Core.context.Users.Where(u => u.Email == email).FirstOrDefault();
            return user!=null;
        }
    }
}
