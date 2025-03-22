using GardenKeeper.Model;
using GardenKeeper.View.ManagerView;
using GardenKeeper.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        private int imageIndex = 0;
        private List<ProductImages> images = new List<ProductImages>();
        Products product;

        Users currentUser;
        public ProductCard(Products product, bool isSell, bool isCustomize, Users user)
        {
            InitializeComponent();
            DataContext = product;
            this.product = product;
            this.isSell= isSell;
            currentUser = user;
            this.isCustomize=isCustomize;

            product.MainImage = Core.context.ProductImages.Where(img => img.ProductId == product.Id).ToList()[0].Image;

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

            images = Core.context.ProductImages.Where(img => img.ProductId == product.Id).ToList();

            ChangeImage();
        }

        private void BuyProductButton_Click(object sender, RoutedEventArgs e)
        {
            QuantitySelectionWindow window = new QuantitySelectionWindow();
            bool? success = window.ShowDialog();
            if (success == true)
            {
                Products existingProduct = ShoppingCardViewModel.Products.FirstOrDefault(p => p.Id == product.Id);
                if (existingProduct == null)
                {
                    ShoppingCardViewModel.Products.Add(product);
                }
                product.SelectedQuantity += QuantitySelectionViewModel.SelectedQuantity;
            }
        }

        private void Image_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void CustomizeButtonImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProductCustomizationWindow window = new ProductCustomizationWindow(product, currentUser);
            window.ShowDialog();
        }

        private void ChangeImageButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            if (imageIndex == 0)
            {
                imageIndex = images.Count;
            }
            imageIndex--;
            ChangeImage();
        }

        private void ChangeImageButtonRight_Click(object sender, RoutedEventArgs e)
        {
            if (imageIndex == images.Count - 1)
            {
                imageIndex = -1;
            }
            imageIndex++;
            ChangeImage();
        }

        private void ChangeImage()
        {
            byte[] imageBytes = images[imageIndex].Image;

            using (var ms = new MemoryStream(imageBytes))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();

                ProductImage.Source = bitmap;
            }
        }
    }
}
