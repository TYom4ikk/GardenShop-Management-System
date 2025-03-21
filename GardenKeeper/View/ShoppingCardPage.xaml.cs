using GardenKeeper.Model;
using GardenKeeper.View.Partial;
using GardenKeeper.View.UsersView;
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

namespace GardenKeeper.View
{
    /// <summary>
    /// Логика взаимодействия для ShoppingCardPage.xaml
    /// </summary>
    public partial class ShoppingCardPage : Page
    {
        Users user;
        private bool isRegisteredUser;
        public ShoppingCardPage(Users user)
        {
            InitializeComponent();
            this.user = user;

            isRegisteredUser = user.Email != null ? true : false;
            LoginButton.Content = isRegisteredUser ? "Войти в другой аккаунт" : "Войти в аккаунт";

            InitializeProductList();
        }

        private void InitializeProductList()
        {
            foreach(var product in ShoppingCardViewModel.Products)
            {
                ShoppingCardProduct productCard = new ShoppingCardProduct(product, ProductListStackPanel);
                productCard.Height = 100;
                productCard.Margin = new Thickness(0,0,0,20);
                ProductListStackPanel.Children.Add(productCard);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPage page = new RegistrationPage();
            this.NavigationService.Navigate(page);
        }

        private void ShoppingCardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogoImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CatalogPage page = new CatalogPage(user);
            this.NavigationService.Navigate(page);
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if(!isRegisteredUser)
            {
                MessageBox.Show("Войдите в аккаунт, чтобы совершить покупку!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(ShoppingCardViewModel.Products.Count == 0)
            {
                MessageBox.Show("Корзина пуста!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (var product in ShoppingCardViewModel.Products)
            {
                var sale = new Sales
                {
                    ProductId = product.Id,
                    Quantity = product.SelectedQuantity,
                    SaleDate = DateTime.Now,
                    UnitPrice = (long)(product.DiscountPrice == null ? product.MainPrice : product.DiscountPrice),
                };
                sale.TotalPrice = (sale.UnitPrice * (long)sale.Quantity);

                Core.context.Sales.Add(sale);
            }
            Core.context.SaveChanges();
            ShoppingCardViewModel.Products.Clear();
            MessageBox.Show("Товары приобретены!", "Покупка совершена!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
