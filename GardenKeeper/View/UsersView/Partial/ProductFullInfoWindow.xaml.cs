using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GardenKeeper.View.UsersView.Partial
{
    /// <summary>
    /// Окно для отображения полной информации о товаре
    /// </summary>
    public partial class ProductFullInfoWindow : Window
    {
        private int imageIndex = 0;
        private List<ProductImages> images = new List<ProductImages>();
        private Products product;

        /// <summary>
        /// Инициализирует новый экземпляр окна с информацией о товаре
        /// </summary>
        /// <param name="product">Товар, информацию о котором нужно отобразить</param>
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

            foreach(ProductProperty property in productProperties)
            {
                var currentProperty = Core.context.Properties.FirstOrDefault(p=>p.Id == property.PropertyId);
                
                // Создаем контейнер для свойства
                StackPanel propertyContainer = new StackPanel();
                propertyContainer.Orientation = Orientation.Horizontal;
                propertyContainer.Margin = new Thickness(0, 5, 0, 5);

                // Создаем TextBlock для названия свойства
                TextBlock propertyName = new TextBlock();
                propertyName.Text = currentProperty.Name + ": ";
                propertyName.FontWeight = FontWeights.Bold;
                propertyName.Width = 200;
                propertyName.VerticalAlignment = VerticalAlignment.Center;

                // Создаем TextBlock для значения свойства
                TextBlock propertyValue = new TextBlock();
                propertyValue.Text = property.Value;
                propertyValue.VerticalAlignment = VerticalAlignment.Center;

                // Добавляем элементы в контейнер
                propertyContainer.Children.Add(propertyName);
                propertyContainer.Children.Add(propertyValue);

                // Добавляем контейнер в StackPanelProperties
                StackPanelProperties.Children.Add(propertyContainer);
            }

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

        /// <summary>
        /// Обработчик нажатия кнопки для переключения на предыдущее изображение
        /// </summary>
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

        /// <summary>
        /// Обработчик нажатия кнопки для переключения на следующее изображение
        /// </summary>
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

        /// <summary>
        /// Обновляет отображаемое изображение товара
        /// </summary>
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