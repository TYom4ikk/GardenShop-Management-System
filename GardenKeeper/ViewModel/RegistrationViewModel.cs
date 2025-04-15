using GardenKeeper.Model;
using GardenKeeper.View.UsersView;
using System.Text;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GardenKeeper.ViewModel
{
    public class RegistrationViewModel
    {
        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="Login"></param>
        /// <param name="Password"></param>
        /// <returns>Аутентифицированный пользователь</returns>
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

        /// <summary>
        /// Проверка email на валидность
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true, если email валидный, иначе false</returns>
        public bool IsEmailValid(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            if(Regex.IsMatch(email, emailPattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Получение пользователя по email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Пользователь полученный по email</returns>
        public Users GetUserByEmail(string email)
        {
            return Core.context.Users.FirstOrDefault(u => u.Email == email);
        }
        /// <summary>
        /// Добавление пользователя в базу данных
        /// </summary>
        /// <param name="user"></param>
        public void AddUser (Users user)
        {
            Core.context.Users.Add(user);
            Core.context.SaveChanges();
        }
        /// <summary>
        /// Генерация случайного пароля
        /// </summary>
        /// <param name="length"></param>
        /// <returns>Сгенерированный пароль</returns>
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
        /// <summary>
        /// Хеширование MD5
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Хеш от пароля</returns>
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
        /// <summary>
        /// Сохранение регистрационных данных в базу данных
        /// </summary>
        /// <param name="email"></param>
        /// <param name="hash"></param>
        public void SaveUserRegistrationData(string email, string hash)
        {
            var user = Core.context.Users.Where(u=>u.Email == email).FirstOrDefault();
            user.PasswordHash = hash;
            Core.context.SaveChanges();
        }

        /// <summary>
        /// Проверка на существование пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Существует ли пользователь</returns>
        public bool IsUserExists(string email)
        {
            var user = Core.context.Users.Where(u => u.Email == email).FirstOrDefault();
            return user!=null;
        }
    }
}
