using GardenKeeper.Model;
using GardenKeeper.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GardenKeeper.View.SystemAdminView
{
    /// <summary>
    /// Логика взаимодействия для AddNewManagerWindow.xaml
    /// </summary>
    public partial class AddNewManagerWindow : Window
    {
        private const int MANAGER_ID = 2;
        public AddNewManagerWindow()
        {
            InitializeComponent();
        }

        private void AddNewManager_Click(object sender, RoutedEventArgs e)
        {
            RegistrationViewModel model = new RegistrationViewModel();
            try
            {
                if (!string.IsNullOrEmpty(EmailTextBox.Text) &&
                    !string.IsNullOrEmpty(PasswordTextBox.Text))
                {
                    Core.context.Users.Add(new Users
                    {
                        Email = EmailTextBox.Text,
                        PasswordHash = model.GenerateHash(PasswordTextBox.Text),
                        UserTypeId = MANAGER_ID
                    });
                    Core.context.SaveChanges();
                    MessageBox.Show($"Менеджер: {EmailTextBox.Text} создан!", "Создан менеджер", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
