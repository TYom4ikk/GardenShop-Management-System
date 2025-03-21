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
        public ShoppingCardProduct(Products product, StackPanel parent)
        {
            InitializeComponent();
            DataContext = product;
            this.product = product;
            ParentContainer = parent;

            PriceTextBlock.Text = product.DiscountPrice == null ?
                product.MainPrice.ToString() : product.DiscountPrice.ToString();

            QuantityTextBlock.Text += product.SelectedQuantity.ToString();
        }

        private Panel ParentContainer { get; set; } // Ссылка на родительский контейнер

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
            ShoppingCardViewModel.Products.Remove(product);
            ParentContainer.Children.Remove(this);
        }
    }
}