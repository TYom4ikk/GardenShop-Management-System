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
        ProductCardViewModel model;
        Products product;

        Users currentUser;

        /// <summary>
        /// Инициализирует новый экземпляр класса ProductCard
        /// </summary>
        /// <param name="product">Товар для отображения</param>
        /// <param name="isSell">Флаг возможности продажи</param>
        /// <param name="isCustomize">Флаг возможности настройки</param>
        /// <param name="user">Текущий пользователь</param>
        public ProductCard(Products product, bool isSell, bool isCustomize, Users user)
        {
            InitializeComponent();
            model = new ProductCardViewModel();
            DataContext = product;
            this.product = product;
            this.isSell= isSell;
            currentUser = user;
            this.isCustomize=isCustomize;

            if (model.GetProductImagesByProductId(product.Id).Count > 0)
            {
                product.MainImage = model.GetProductImagesByProductId(product.Id)[0].Image;
            }

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

            images = model.GetProductImagesByProductId(product.Id);
            if (images.Count > 0) 
            ChangeImage();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку покупки товара
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void BuyProductButton_Click(object sender, RoutedEventArgs e)
        {
            QuantitySelectionWindow window = new QuantitySelectionWindow();
            bool? success = window.ShowDialog();
            if (success == true)
            {
                if(QuantitySelectionViewModel.SelectedQuantity > product.Quantity)
                {
                    MessageBox.Show("Неверное количество выбранных товаров!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Products existingProduct = ShoppingCardViewModel.Products.FirstOrDefault(p => p.Id == product.Id);
                if (existingProduct == null)
                {
                    ShoppingCardViewModel.Products.Add(product);
                }
                product.SelectedQuantity += QuantitySelectionViewModel.SelectedQuantity;
            }
        }

        /// <summary>
        /// Обработчик обратной связи при перетаскивании изображения
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void Image_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        /// <summary>
        /// Обработчик нажатия на кнопку настройки товара
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void CustomizeButtonImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProductCustomizationWindow window = new ProductCustomizationWindow(product, currentUser);
            window.ShowDialog();
            ProductDiscountPriceTextBlock.Text = product.FormattedDiscountPrice;
            ProductMainPriceTextBlock.Text = product.FormattedMainPrice;
            ProductNameTextBlock.Text = product.Name;
            ProductDescriptionTextBlock.Text = product.Description;
            ProductQuantityTextBlock.Text = $"Осталось: {product.Quantity} шт.";
        }

        /// <summary>
        /// Обработчик нажатия на кнопку переключения изображения влево
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void ChangeImageButtonLeft_Click(object sender, RoutedEventArgs e)
        {
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
            if (imageIndex == images.Count - 1)
            {
                imageIndex = -1;
            }
            imageIndex++;
            ChangeImage();
        }

        /// <summary>
        /// Изменяет текущее отображаемое изображение товара
        /// </summary>
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

        /// <summary>
        /// Обработчик нажатия на кнопку информации о товаре
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void InfoProductButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Открывает окно с полной информацией о товаре
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void OpenProductFullInfoWindow(object sender, MouseButtonEventArgs e)
        {
            var window = new ProductFullInfoWindow(product);
            window.ShowDialog();
        }
    }
}
