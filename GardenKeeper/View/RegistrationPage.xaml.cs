using GardenKeeper.Model;
using GardenKeeper.View.UsersView;
using GardenKeeper.ViewModel;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GardenKeeper.View
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        RegistrationViewModel model;
        public RegistrationPage()
        {
            InitializeComponent();
            model = new RegistrationViewModel();
        }

        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
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

        private void ContinueWithoutRegistration_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Users user = new Users();
            user.UserTypeId = 1;
            CatalogPage page = new CatalogPage(user);
            this.NavigationService.Navigate(page);
        }

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
    }
}
