using GardenKeeper.Model;
using GardenKeeper.View.UsersView;
using GardenKeeper.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GardenKeeper.View
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();

            DataContext = new RegistrationViewModel();
        }

        private void ButtonSubmit_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(TextBoxLogin.Text) && !string.IsNullOrEmpty(PasswordBoxPassword.Password))
            {
                var currentUser = Authentication.IsAuthenticated(TextBoxLogin.Text, PasswordBoxPassword.Password);
                if (currentUser == null)
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка аутентификации", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                CatalogPage page = new CatalogPage(currentUser);
                this.NavigationService.Navigate(page);
            }
        }

        private void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Users user = new Users();
            CatalogPage page = new CatalogPage(user);
            this.NavigationService.Navigate(page);
        }
    }
}
