﻿using GardenKeeper.Model;
using GardenKeeper.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GardenKeeper.View
{
    /// <summary>
    /// Окно для отображения полной информации о товаре
    /// </summary>
    public partial class ProductFullInfoWindow : Window
    {
        private int imageIndex = 0;
        private List<ProductImages> images = new List<ProductImages>();
        private Products product;
        ProductFullInfoViewModel model;

        /// <summary>
        /// Инициализирует новое окно с информацией о товаре
        /// </summary>
        /// <param name="product">Товар, информацию о котором нужно отобразить</param>
        public ProductFullInfoWindow(Products product)
        {
            InitializeComponent();

            model = new ProductFullInfoViewModel();

            this.product = product;
            DataContext = product;

            // Загрузка изображений
            images = model.GetImagesByProductId(product.Id);
            if (images.Count > 0)
            {
                ChangeImage();
            }

            // Загрузка свойств товара
            var productProperties = model.GetProductPropertyByProductId(product.Id);

            foreach (ProductProperty property in productProperties)
            {
                var currentProperty = model.GetPropertyById(property.PropertyId);

                StackPanel propertyContainer = new StackPanel();
                propertyContainer.Orientation = Orientation.Horizontal;
                propertyContainer.Margin = new Thickness(0, 20, 0, 20);

                TextBlock propertyName = new TextBlock();
                propertyName.Text = currentProperty.Name + ": ";
                propertyName.FontWeight = FontWeights.Bold;
                propertyName.Width = 200;
                propertyName.VerticalAlignment = VerticalAlignment.Center;
                propertyName.FontSize = 20;
                propertyName.Foreground = Brushes.White;
                propertyName.Margin = new Thickness(30, 0, 0, 0);

                TextBlock propertyValue = new TextBlock();
                propertyValue.Text = currentProperty.Value;
                propertyValue.VerticalAlignment = VerticalAlignment.Center;
                propertyValue.FontSize = 20;
                propertyValue.Foreground = Brushes.White;
                propertyValue.Margin = new Thickness(180, 0, 0, 0);

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
        /// Обработчик нажатия на кнопку переключения изображения влево
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
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
        /// Обработчик нажатия на кнопку переключения изображения вправо
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
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
        /// Изменяет отображаемое изображение товара
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