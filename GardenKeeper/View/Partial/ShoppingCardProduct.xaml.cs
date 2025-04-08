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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GardenKeeper.View.Partial
{
    /// <summary>
    /// Логика взаимодействия для ShoppingCardProduct.xaml
    /// </summary>
    public partial class ShoppingCardProduct : UserControl
    {
        private Products product;

        /// <summary>
        /// Инициализирует новый экземпляр класса ShoppingCardProduct
        /// </summary>
        /// <param name="product">Товар для отображения в корзине</param>
        /// <param name="parent">Родительский контейнер для размещения элемента</param>
        public ShoppingCardProduct(Products product, StackPanel parent)
        {
            InitializeComponent();
            DataContext = product;
            this.product = product;
            ParentContainer = parent;

            PriceTextBlock.Text = product.DiscountPrice == null ?
                product.MainPrice.ToString() : product.DiscountPrice.ToString();

            QuantityTextBox.Text = QuantitySelectionViewModel.SelectedQuantity.ToString();
        }

        /// <summary>
        /// Ссылка на родительский контейнер
        /// </summary>
        private Panel ParentContainer { get; set; }

        /// <summary>
        /// Обработчик нажатия на кнопку удаления товара из корзины
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ShoppingCardViewModel.Products.Remove(product);
            ParentContainer.Children.Remove(this);
        }

        /// <summary>
        /// Обработчик нажатия на кнопку увеличения количества товара
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                QuantityTextBox.Text = (quantity + 1).ToString();
                product.SelectedQuantity = quantity + 1;
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку уменьшения количества товара
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantityTextBox.Text, out int quantity) && quantity > 1)
            {
                QuantityTextBox.Text = (quantity - 1).ToString();
                product.SelectedQuantity = quantity - 1;
            }
        }
    }
}