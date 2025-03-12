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

namespace GardenKeeper.View.UsersView.Partial
{
    /// <summary>
    /// Логика взаимодействия для ProductCard.xaml
    /// </summary>
    public partial class ProductCard : UserControl
    {
        private bool isSell;
        public ProductCard(Products product, bool isSell)
        {
            InitializeComponent();
            DataContext = product;
            this.isSell= isSell;

            if(product.DiscountPrice == null)
            {
                ProductMainPriceTextBlock.FontSize = 30;
                ProductMainPriceTextBlock.TextDecorations = null;
            }

            if(product.Quantity == 0)
            {
                ProductQuantityTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                ProductQuantityTextBlock.Text = "Нет в наличии";
            }
        }

        private void BuyProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isSell)
            {
                MessageBox.Show("Войдите в аккаунт!");
            }
            else
            {
                MessageBox.Show("Продано!");
            }
        }
    }
}
