using GardenKeeper.Model;
using GardenKeeper.View.ManagerView;
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

namespace GardenKeeper.View.UsersView.Partial
{
    /// <summary>
    /// Логика взаимодействия для ProductCard.xaml
    /// </summary>
    public partial class ProductCard : UserControl
    {
        private bool isSell;
        private bool isCustomize;
        public ProductCard(Products product, bool isSell, bool isCustomize)
        {
            InitializeComponent();
            DataContext = product;
            this.isSell= isSell;
            this.isCustomize=isCustomize;

            CustomizeButtonImage.Visibility = isCustomize ? Visibility.Visible : Visibility.Hidden;

            if (product.DiscountPrice == null)
            {
                ProductMainPriceTextBlock.FontSize = 30;
                ProductMainPriceTextBlock.TextDecorations = null;
            }

            if(product.Quantity == 0)
            {
                ProductQuantityTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                ProductQuantityTextBlock.Text = "Нет в наличии";
                BuyProductButton.IsEnabled = false;
            }
        }

        private void BuyProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isSell)
            {
                MessageBox.Show("Чтобы приобретать товары, войдите в аккаунт!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ProductWindow window = new ProductWindow();
                window.ShowDialog();
            }
        }

        private void Image_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void CustomizeButtonImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProductCustomizationWindow window = new ProductCustomizationWindow(DataContext as Products);
            window.ShowDialog();
        }
    }
}
