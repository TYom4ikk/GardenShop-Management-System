﻿using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using GardenKeeper.Model;
using GardenKeeper.View.UsersView;
using GardenKeeper.ViewModel;

namespace GardenKeeper.View
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        RegistrationViewModel model;
        bool isRegistrationMode = false;

        /// <summary>
        /// Инициализирует новый экземпляр класса RegistrationPage
        /// </summary>
        public RegistrationPage()
        {
            InitializeComponent();
            model = new RegistrationViewModel();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку подтверждения
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!isRegistrationMode)
            {
                Users currentUser = (model).Authenticate(TextBoxLogin.Text, PasswordBoxPassword.Password);
                if (currentUser == null)
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка аутентификации", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    CatalogPage page = new CatalogPage(currentUser);
                    this.NavigationService.Navigate(page);
                }
            }
            else
            {
                string email = TextBoxLogin.Text;
                string password = PasswordBoxPassword.Password;

                if (model.IsEmailValid(email) && !string.IsNullOrEmpty(password))
                {
                    if(model.GetUserByEmail(email) != null)
                    {
                        MessageBox.Show("Такой email уже зарегестрирован в системе!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;

                    }
                    Users currentUser = new Users
                    {
                        Email = email,
                        PasswordHash = model.GenerateHash(password),
                        UserTypeId = 1
                    };
                   
                    model.AddUser(currentUser);
                    MessageBox.Show("Вы зарегестрировались!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                    CatalogPage page = new CatalogPage(currentUser);
                    this.NavigationService.Navigate(page);
                }
                else
                {
                    MessageBox.Show("Введите корректные данные!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку продолжения без регистрации
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void ContinueWithoutRegistration_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Users user = new Users();
            user.UserTypeId = 1;
            CatalogPage page = new CatalogPage(user);
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Обработчик нажатия на кнопку восстановления пароля
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void ForgetPassword_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text))
            {
                MessageBox.Show("Введите Email, для отправки авторизационных данных на вашу электронную почту.", "Сброс пароля", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (!model.IsUserExists(TextBoxLogin.Text))
                {
                    MessageBox.Show("Пользователя с такой почтой не существует!");
                    return;
                }
                string newPassword = model.GeneratePassword(16);
                string hash = model.GenerateHash(newPassword);
                model.SaveUserRegistrationData(TextBoxLogin.Text, hash);
                EmailInteraction.SendResetPassword(TextBoxLogin.Text, newPassword);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку переключения режима регистрации/входа
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void RegistrationButton_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isRegistrationMode = !isRegistrationMode;
            if (isRegistrationMode)
            {
                ButtonSubmit.Content = "Зарегестрироваться";
                RegistrationButton.Content = "Войти в существующий аккаунт";
            }
            else
            {
                ButtonSubmit.Content = "Войти";
                RegistrationButton.Content = "Зарегестрироваться";
            }
        }
    }
}
