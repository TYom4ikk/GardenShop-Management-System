using GardenKeeper.Model;
using GardenKeeper.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private const string EMAIL_PATTERN = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        AddNewManagerViewModel model;

        /// <summary>
        /// Инициализирует новый экземпляр окна добавления менеджера
        /// </summary>
        public AddNewManagerWindow()
        {
            InitializeComponent();
            model = new AddNewManagerViewModel();
        }

        /// <summary>
        /// Проверяет корректность email адреса
        /// </summary>
        /// <param name="email">Email адрес для проверки</param>
        /// <returns>True, если email корректен, иначе False</returns>
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, EMAIL_PATTERN);
        }

        /// <summary>
        /// Обработчик нажатия на кнопку добавления нового менеджера
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void AddNewManager_Click(object sender, RoutedEventArgs e)
        {
            RegistrationViewModel regModel = new RegistrationViewModel();
            try
            {
                if (!string.IsNullOrEmpty(EmailTextBox.Text) &&
                    !string.IsNullOrEmpty(PasswordTextBox.Text))
                {
                    if (!IsValidEmail(EmailTextBox.Text))
                    {
                        MessageBox.Show("Введите корректный email адрес!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    model.AddNewManager(new Users
                    {
                        Email = EmailTextBox.Text,
                        PasswordHash = regModel.GenerateHash(PasswordTextBox.Text),
                        UserTypeId = MANAGER_ID
                    });
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
