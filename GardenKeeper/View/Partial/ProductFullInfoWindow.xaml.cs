using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GardenKeeper.View.UsersView.Partial
{
    public partial class ProductFullInfoWindow : Window
    {
        private int imageIndex = 0;
        private List<ProductImages> images = new List<ProductImages>();
        private Products product;

        public ProductFullInfoWindow(Products product)
        {
            InitializeComponent();
            this.product = product;
            DataContext = product;

            // Загрузка изображений
            images = Core.context.ProductImages.Where(img => img.ProductId == product.Id).ToList();
            if (images.Count > 0)
            {
                ChangeImage();
            }

            // Загрузка свойств товара
            var productProperties = Core.context.ProductProperty
                .Where(pp => pp.ProductId == product.Id)
                .ToList();
            PropertiesItemsControl.ItemsSource = productProperties;

            // Настройка отображения цен
            if (product.DiscountPrice == null)
            {
                ProductMainPriceTextBlock.FontSize = 30;
                ProductMainPriceTextBlock.TextDecorations = null;
            }

            // Настройка отображения количества
            if (product.Quantity == 0)
            {
                ProductQuantityTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                ProductQuantityTextBlock.Text = "Нет в наличии";
            }
        }

        private void ChangeImageButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            if (images.Count == 0) return;

            if (imageIndex == 0)
            {
                imageIndex = images.Count;
            }
            imageIndex--;
            ChangeImage();
        }

        private void ChangeImageButtonRight_Click(object sender, RoutedEventArgs e)
        {
            if (images.Count == 0) return;

            if (imageIndex == images.Count - 1)
            {
                imageIndex = -1;
            }
            imageIndex++;
            ChangeImage();
        }

        private void ChangeImage()
        {
            if (images.Count == 0) return;

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