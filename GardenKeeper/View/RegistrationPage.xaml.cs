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
            if(true) //!string.IsNullOrEmpty(TextBoxLogin.Text) && !string.IsNullOrEmpty(PasswordBoxPassword.Password)
            {
                if(true) //Authentication.IsAuthenticated(TextBoxLogin.Text, PasswordBoxPassword.Password)
                {
                    this.NavigationService.Navigate(new Uri("View\\UsersView\\CatalogPage.xaml", UriKind.Relative)); //View\\MainPage.xaml
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка аутентификации", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
