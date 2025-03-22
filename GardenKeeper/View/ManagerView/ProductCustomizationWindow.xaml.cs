using GardenKeeper.Model;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GardenKeeper.View.ManagerView
{
    /// <summary>
    /// Логика взаимодействия для ProductCustomizationWindow.xaml
    /// </summary>
    public partial class ProductCustomizationWindow : Window
    {
        ProductCustomizationViewModel model;
        Products currentProduct;
        Products oldProduct;
        KeyValuePair<TextBox, TextBox> ProperyNameValue;
        private int imageIndex = 0;
        private List<ProductImages> images = new List<ProductImages>();

        private Users currentUser;
        public ProductCustomizationWindow(Products product, Users user)
        {
            InitializeComponent();
            DataContext = product;
            currentProduct = product;
            oldProduct = product;
            model = new ProductCustomizationViewModel();
            currentUser = user;

            UpdateImages();

            using (var ms = new MemoryStream(product.MainImage))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                ProductImage.Source = bitmap;
            }
            NameTextBox.Text = product.Name;
            DescriptionTextBox.Text = product.Description;
            MainPriceTextBox.Text = product.FormattedMainPrice;
            DiscountPirceTextBox.Text = product.FormattedDiscountPrice;

            CategoryComboBox.ItemsSource = Core.context.Categories.ToList();
            CategoryComboBox.DisplayMemberPath = "Name";
            CategoryComboBox.SelectedIndex = 0;
        }

        private void UpdateImages()
        {
            images = Core.context.ProductImages.Where(img => img.ProductId == currentProduct.Id).ToList();
            imageIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //model.AddNewProperty();
        }

        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {


            var currentStackPanel = (sender as Button).Parent as StackPanel;
            var parentStackPanel = currentStackPanel.Parent as StackPanel;

            currentStackPanel

            // Удаляем кнопку добавления из текущей панели
            currentStackPanel.Children.Remove(sender as Button);

            // Создаем новую панель свойств
            StackPanel newPropertyPanel = new StackPanel();
            newPropertyPanel.Orientation = Orientation.Horizontal;
            newPropertyPanel.Style = (Style)FindResource("ProperyRow");

            // Создаем контейнер для названия свойства
            StackPanel nameContainer = new StackPanel();
            Label nameLabel = new Label();
            nameLabel.Content = "Название свойства";
            TextBox propertyNameTextBox = new TextBox();
            propertyNameTextBox.TextChanged += PropertyName_TextChanged;
            nameContainer.Children.Add(nameLabel);
            nameContainer.Children.Add(propertyNameTextBox);

            // Создаем контейнер для значения свойства
            StackPanel valueContainer = new StackPanel();
            Label valueLabel = new Label();
            valueLabel.Content = "Значение свойства";
            TextBox propertyValueTextBox = new TextBox();
            propertyValueTextBox.TextChanged += PropertyValue_TextChanged;
            valueContainer.Children.Add(valueLabel);
            valueContainer.Children.Add(propertyValueTextBox);

            // Создаем кнопку добавления нового свойства
            Button addPropertyButton = new Button();
            addPropertyButton.Content = "+";
            addPropertyButton.Width = 30;
            addPropertyButton.Click += AddProperty_Click;

            // Добавляем элементы в новую панель
            newPropertyPanel.Children.Add(nameContainer);
            newPropertyPanel.Children.Add(valueContainer);
            newPropertyPanel.Children.Add(addPropertyButton);

            // Добавляем новую панель в родительский контейнер
            parentStackPanel.Children.Add(newPropertyPanel);


        }

        private void PropertyName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PropertyValue_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DeleteImage_Click(object sender, RoutedEventArgs e)
        {
            if (model.DeleteImage(images[imageIndex].Id) == true)
            {
                MessageBox.Show("Картинка удалена!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Произошла ошибка при удалении!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateImages();
            ChangeImage();

            var log = new AuditLog
            {
                ProductId = currentProduct.Id,
                ActionId = 2, // DELETE
                FieldId = 7, // IMAGE
                OldValue = images[imageIndex].Image.ToString(),
                NewValue = null,
                ChangeDate = DateTime.Now,
                UserId = currentUser.Id
            };
            Core.context.AuditLog.Add(log);
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            if (model.AddNewImage(currentProduct.Id) == true)
            {
                MessageBox.Show("Картинка добавлена!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else {
                MessageBox.Show("Произошла ошибка при добавлении!" ,"Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateImages();
            ChangeImage();

            var log = new AuditLog
            {
                ProductId = currentProduct.Id,
                ActionId = 1, // INSERT
                FieldId = 7, // IMAGE
                OldValue = null,
                NewValue = images[imageIndex].Image.ToString(),
                ChangeDate = DateTime.Now,
                UserId = currentUser.Id
            };
            currentProduct.Name = NameTextBox.Text;
            Core.context.AuditLog.Add(log);
        }

        // Можно оставить пустым (Описание, скидочную цену)
        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NameTextBox.Text) &&
                !string.IsNullOrEmpty(DescriptionTextBox.Text) &&
                !string.IsNullOrEmpty(QuantityTextBox.Text)
                )
            {
            
                
                var log = new AuditLog
                {
                    ProductId = currentProduct.Id,
                    ActionId = 3, // UPDATE
                    FieldId = 1, // NAME
                    OldValue = currentProduct.Name,
                    NewValue = NameTextBox.Text,
                    ChangeDate = DateTime.Now,
                    UserId = currentUser.Id
                };
                currentProduct.Name = NameTextBox.Text;
                Core.context.AuditLog.Add(log);
                
                currentProduct.Description = DescriptionTextBox.Text;

                currentProduct.MainPrice = MainPriceTextBox.Text;

                if (!string.IsNullOrEmpty(DiscountPirceTextBox.Text))
                {
                    currentProduct.DiscountPrice = DiscountPirceTextBox.Text;
                }
                currentProduct.Quantity = QuantityTextBox.Text;
                currentProduct.CategoryId = CategoryComboBox.SelectedItem;


                Core.context.SaveChanges();
            }
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
