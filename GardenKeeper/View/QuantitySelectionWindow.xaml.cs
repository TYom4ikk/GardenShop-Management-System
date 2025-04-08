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

namespace GardenKeeper.View
{
    /// <summary>
    /// Логика взаимодействия для QuantitySelectionWindow.xaml
    /// </summary>
    public partial class QuantitySelectionWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса QuantitySelectionWindow
        /// </summary>
        public QuantitySelectionWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку подтверждения выбора количества
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantityTextBox.Text, out int quantity) && quantity>0)
            {
                DialogResult = true;
                QuantitySelectionViewModel.SelectedQuantity = quantity;
            }
            else
            {
                MessageBox.Show("Введите корректное число!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку отмены
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку увеличения количества
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                QuantityTextBox.Text = (quantity + 1).ToString();
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку уменьшения количества
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantityTextBox.Text, out int quantity) && quantity > 1)
            {
                QuantityTextBox.Text = (quantity - 1).ToString();
            }
        }

        /// <summary>
        /// Получает выбранное количество товара
        /// </summary>
        public int SelectedQuantity
        {
            get
            {
                int.TryParse(QuantityTextBox.Text, out int quantity);
                return quantity;
            }
        }
    }
}
