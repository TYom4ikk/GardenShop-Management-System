using GardenKeeper.Model;
using GardenKeeper.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                if(Authentication.IsAuthenticated(TextBoxLogin.Text, PasswordBoxPassword.Password))
                {
                    this.NavigationService.Navigate(new Uri("View\\UsersView\\CatalogPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Пароль неверный!");
                }
            }
        }
    }
}
