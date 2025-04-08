using GardenKeeper.Model;
using GardenKeeper.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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

        private Dictionary<TextBox, TextBox> propertyPairs = new Dictionary<TextBox, TextBox>();
        private int imageIndex = 0;
        private List<ProductImages> images = new List<ProductImages>();

        private Users currentUser;

        /// <summary>
        /// Инициализирует новый экземпляр окна настройки товара
        /// </summary>
        /// <param name="product">Товар для настройки</param>
        /// <param name="user">Текущий пользователь</param>
        public ProductCustomizationWindow(Products product, Users user)
        {
            InitializeComponent();
            
            propertyPairs = new Dictionary<TextBox, TextBox>();
            
            DataContext = product;
            currentProduct = product;
            model = new ProductCustomizationViewModel();
            currentUser = user;

            UpdateImagesCollection();

            if (product.MainImage != null)
            {
                using (var ms = new MemoryStream(product.MainImage))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    ProductImage.Source = bitmap;
                }
            }

            NameTextBox.Text = product.Name;
            DescriptionTextBox.Text = product.Description;
            MainPriceTextBox.Text = product.FormattedMainPrice;
            DiscountPirceTextBox.Text = product.FormattedDiscountPrice;
            QuantityTextBox.Text = product.Quantity.ToString();

            CategoryComboBox.ItemsSource = model.GetCategories();
            CategoryComboBox.DisplayMemberPath = "Name";
            
            for (int i = 0; i < CategoryComboBox.Items.Count; i++)
            {
                var category = CategoryComboBox.Items[i] as Categories;
                if (category != null && category.Id == product.CategoryId)
                {
                    CategoryComboBox.SelectedIndex = i;
                    break;
                }
            }
            
            if (CategoryComboBox.SelectedIndex < 0)
                CategoryComboBox.SelectedIndex = 0;
            
            LoadExistingProperties();
           
            UpdatePropertyCountDisplay();
        }

        /// <summary>
        /// Загружает существующие свойства товара
        /// </summary>
        private void LoadExistingProperties()
        {
            try
            {
                var productProperties = model.GetProductPropertiesByProductId(currentProduct.Id);
                   
                
                if (productProperties.Count > 0)
                {
                    // Используем существующую панель CustomPropertiesStackPanel
                    foreach (var productProperty in productProperties)
                    {
                        // Получаем свойство
                        var property = model.GetPropertyById(productProperty.PropertyId);
                        if (property != null)
                        {
                            
                            StackPanel newPropertyPanel = new StackPanel();
                            newPropertyPanel.Orientation = Orientation.Horizontal;

                            // Создаем контейнер для названия свойства
                            StackPanel nameContainer = new StackPanel();
                            Label nameLabel = new Label();
                            nameLabel.Content = "Название свойства";
                            TextBox propertyNameTextBox = new TextBox();
                            propertyNameTextBox.Text = property.Name;
                            propertyNameTextBox.TextChanged += PropertyName_TextChanged;
                            nameContainer.Children.Add(nameLabel);
                            nameContainer.Children.Add(propertyNameTextBox);

                            // Создаем контейнер для значения свойства
                            StackPanel valueContainer = new StackPanel();
                            Label valueLabel = new Label();
                            valueLabel.Content = "Значение свойства";
                            TextBox propertyValueTextBox = new TextBox();
                            propertyValueTextBox.Text = property.Value;
                            propertyValueTextBox.TextChanged += PropertyValue_TextChanged;
                            valueContainer.Children.Add(valueLabel);
                            valueContainer.Children.Add(propertyValueTextBox);

                            // Добавляем пару в словарь
                            propertyPairs[propertyNameTextBox] = propertyValueTextBox;
                            
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
                            CustomPropertiesStackPanel.Children.Add(newPropertyPanel);
                        }
                    }
                    
                    // После загрузки всех свойств удаляем кнопку "+" со всех строк, кроме последней
                    for (int i = 0; i < CustomPropertiesStackPanel.Children.Count - 1; i++)
                    {
                        var panel = CustomPropertiesStackPanel.Children[i] as StackPanel;
                        if (panel != null)
                        {
                            var addButton = panel.Children.OfType<Button>().FirstOrDefault();
                            if (addButton != null)
                            {
                                panel.Children.Remove(addButton);
                            }
                        }
                    }
                    
                    
                    // Обновляем счетчик свойств
                    UpdatePropertyCountDisplay();
                }
                else
                {
                    // Если свойств нет, создаем пустую строку для добавления нового свойства
                    AddEmptyPropertyRow();
                }
            }
            catch (Exception ex)
            {
                
                // В случае ошибки всё равно добавляем пустую строку
                AddEmptyPropertyRow();
            }
        }
        
        /// <summary>
        /// Добавляет пустую строку для нового свойства
        /// </summary>
        private void AddEmptyPropertyRow()
        {
            try
            {
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

                // Добавляем пару в словарь
                propertyPairs[propertyNameTextBox] = propertyValueTextBox;

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
                CustomPropertiesStackPanel.Children.Add(newPropertyPanel);
                
                // Обновляем счетчик свойств
                UpdatePropertyCountDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении пустой строки свойства: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обновляет коллекцию изображений товара
        /// </summary>
        private void UpdateImagesCollection()
        {
            images = model.GetProductImagesByProductId(currentProduct.Id);
            imageIndex = 0;
        }

        /// <summary>
        /// Обновляет отображение количества свойств
        /// </summary>
        private void UpdatePropertyCountDisplay()
        {
            PropertyPairsCountTextBlock.Text = $"Свойства: {propertyPairs.Count}";
        }

        /// <summary>
        /// Обработчик изменения текста в поле названия свойства
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void PropertyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox nameTextBox = sender as TextBox;
            
            try
            {
                // Получаем родительский контейнер (должен быть StackPanel)
                var nameContainer = nameTextBox.Parent as StackPanel;
                if (nameContainer == null)
                {
                    return;
                }
                
                // Получаем контейнер более высокого уровня, содержащий все элементы
                var propertyRow = nameContainer.Parent as StackPanel;
                if (propertyRow == null)
                {
                    return;
                }
                
                // Находим контейнер значения (второй StackPanel в ряду)
                StackPanel valueContainer = null;
                TextBox valueTextBox = null;
                
                int childIndex = 0;
                
                // Получаем все дочерние StackPanel
                foreach (var child in propertyRow.Children)
                {
                    
                    if (child is StackPanel && child != nameContainer)
                    {
                        valueContainer = child as StackPanel;
                        break;
                    }
                    childIndex++;
                }
                
                if (valueContainer == null)
                {
                    return;
                }
                
                // Находим TextBox в контейнере значения
                foreach (var child in valueContainer.Children)
                {
                    
                    if (child is TextBox)
                    {
                        valueTextBox = child as TextBox;
                        break;
                    }
                }
                
                if (valueTextBox == null)
                {
                    return;
                }
                
                // Обновляем словарь
                if (!propertyPairs.ContainsKey(nameTextBox))
                {
                    propertyPairs.Add(nameTextBox, valueTextBox);
                }
                else
                {
                    propertyPairs[nameTextBox] = valueTextBox;
                }
                
                // Обновляем счетчик свойств
                UpdatePropertyCountDisplay();
                
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Обработчик изменения текста в поле значения свойства
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void PropertyValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Аналогично обрабатываем изменение значения свойства
            // Удалим MessageBox и добавим отладочный вывод
            TextBox valueTextBox = sender as TextBox;
            
            try
            {
                // Получаем родительский контейнер (должен быть StackPanel)
                var valueContainer = valueTextBox.Parent as StackPanel;
                if (valueContainer == null) return;
                
                // Получаем контейнер более высокого уровня, содержащий все элементы
                var propertyRow = valueContainer.Parent as StackPanel;
                if (propertyRow == null) return;
                
                // Находим контейнер имени (первый StackPanel в ряду)
                StackPanel nameContainer = null;
                TextBox nameTextBox = null;
                
                // Получаем все дочерние StackPanel
                foreach (var child in propertyRow.Children)
                {
                    if (child is StackPanel && child != valueContainer)
                    {
                        nameContainer = child as StackPanel;
                        break;
                    }
                }
                
                if (nameContainer == null) return;
                
                // Находим TextBox в контейнере имени
                foreach (var child in nameContainer.Children)
                {
                    if (child is TextBox)
                    {
                        nameTextBox = child as TextBox;
                        break;
                    }
                }
                
                if (nameTextBox == null) return;
                
                // Обновляем словарь
                if (!propertyPairs.ContainsKey(nameTextBox))
                {
                    propertyPairs.Add(nameTextBox, valueTextBox);
                }
                else
                {
                    propertyPairs[nameTextBox] = valueTextBox;
                }
                
                // Обновляем счетчик свойств
                UpdatePropertyCountDisplay();
                
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку добавления свойства
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                
                var currentStackPanel = (sender as Button).Parent as StackPanel;
                var parentStackPanel = currentStackPanel.Parent as StackPanel;

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

                // Непосредственно добавляем пару в словарь (без проверки на пустоту)
                propertyPairs[propertyNameTextBox] = propertyValueTextBox;
                
                // Обновляем счетчик свойств
                UpdatePropertyCountDisplay();

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
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Находит родительский элемент указанного типа
        /// </summary>
        /// <typeparam name="T">Тип искомого элемента</typeparam>
        /// <param name="child">Дочерний элемент</param>
        /// <returns>Найденный родительский элемент или null</returns>
        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            // Получаем родителя
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            
            // Если родитель нужного типа - возвращаем его
            if (parentObject is T parent)
                return parent;
                
            // Если это не тот тип родителя, который мы ищем, продолжаем вверх по дереву
            if (parentObject != null)
                return FindVisualParent<T>(parentObject);
                
            return null;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку удаления изображения
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void DeleteImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (model.DeleteImage(images[imageIndex].Id) == true)
            {
                MessageBox.Show("Картинка удалена!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Произошла ошибка при удалении!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateImagesCollection();
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
            model.AddLog(log);
        }

        /// <summary>
        /// Обработчик нажатия на кнопку добавления изображения
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (model.AddNewImage(currentProduct.Id) == true)
            {
                MessageBox.Show("Картинка добавлена!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else {
                MessageBox.Show("Произошла ошибка при добавлении!" ,"Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateImagesCollection();
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
            model.AddLog(log);
        }

        /// <summary>
        /// Обработчик нажатия на кнопку сохранения изменений
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Выводим отладочную информацию о свойствах
                
                foreach (var pair in propertyPairs)
                {
                }
                
                if (!string.IsNullOrEmpty(NameTextBox.Text) &&
                    !string.IsNullOrEmpty(MainPriceTextBox.Text) &&
                    !string.IsNullOrEmpty(QuantityTextBox.Text)
                    )
                {
                    // Сначала обрабатываем основные свойства товара
                    if (currentProduct.Name != NameTextBox.Text)
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
                        model.AddLog(log);
                    }
                    
                    if (!string.IsNullOrEmpty(currentProduct.Description))
                    {
                        if (!string.IsNullOrEmpty(DescriptionTextBox.Text))
                        {
                            if(currentProduct.Description != DescriptionTextBox.Text)
                            {
                                var log = new AuditLog
                                {
                                    ProductId = currentProduct.Id,
                                    ActionId = 3, // UPDATE
                                    FieldId = 2, // DESCRIPTION
                                    OldValue = currentProduct.Description,
                                    NewValue = DescriptionTextBox.Text,
                                    ChangeDate = DateTime.Now,
                                    UserId = currentUser.Id
                                };
                                currentProduct.Description = DescriptionTextBox.Text;
                                model.AddLog(log);
                            }
                        }
                        else
                        {
                            var log = new AuditLog
                            {
                                ProductId = currentProduct.Id,
                                ActionId = 2, // DELETE
                                FieldId = 2, // DESCRIPTION
                                OldValue = currentProduct.Description,
                                NewValue = null,
                                ChangeDate = DateTime.Now,
                                UserId = currentUser.Id
                            };
                            currentProduct.Description = string.Empty;
                            model.AddLog(log);
                        }
                    }
                    else{
                        if (!string.IsNullOrEmpty(DescriptionTextBox.Text))
                        {
                            var log = new AuditLog
                            {
                                ProductId = currentProduct.Id,
                                ActionId = 1, // INSERT
                                FieldId = 2, // DESCRIPTION
                                OldValue = null,
                                NewValue = currentProduct.Description,
                                ChangeDate = DateTime.Now,
                                UserId = currentUser.Id
                            };
                            currentProduct.Description = DescriptionTextBox.Text;
                            model.AddLog(log);
                        }
                    }

                    var newMainPrice = long.Parse(MainPriceTextBox.Text.Split(' ')[0].Replace(".", ""));
                    if (currentProduct.MainPrice != newMainPrice)
                    {
                        var log = new AuditLog
                        {
                            ProductId = currentProduct.Id,
                            ActionId = 3, // UPDATE
                            FieldId = 3, // MAIN PRICE
                            OldValue = currentProduct.FormattedMainPrice.Replace("₽",""),
                            ChangeDate = DateTime.Now,
                            UserId = currentUser.Id
                        };
                        currentProduct.MainPrice = newMainPrice;
                        log.NewValue = currentProduct.FormattedMainPrice.Replace("₽", "");
                        model.AddLog(log);
                    }


                    if (!string.IsNullOrEmpty(currentProduct.DiscountPrice.ToString()))
                    {
                        if (!string.IsNullOrEmpty(DiscountPirceTextBox.Text))
                        {
                            if (currentProduct.DiscountPrice != long.Parse(DiscountPirceTextBox.Text.Split(' ')[0].Replace(".", "")))
                            {
                                var log = new AuditLog
                                {
                                    ProductId = currentProduct.Id,
                                    ActionId = 3, // UPDATE
                                    FieldId = 4, // DISCOUNT PRICE
                                    OldValue = currentProduct.FormattedDiscountPrice.Replace("₽",""),
                                    ChangeDate = DateTime.Now,
                                    UserId = currentUser.Id
                                };
                                currentProduct.DiscountPrice = long.Parse(DiscountPirceTextBox.Text.Split(' ')[0].Replace(".", ""));
                                log.NewValue = currentProduct.FormattedDiscountPrice.Replace("₽","");
                                model.AddLog(log);
                            }
                        }
                        else
                        {
                            var log = new AuditLog
                            {
                                ProductId = currentProduct.Id,
                                ActionId = 2, // DELETE
                                FieldId = 4, // DISCOUNT PIRCE
                                OldValue = currentProduct.FormattedDiscountPrice.Replace("₽", ""),
                                NewValue = null,
                                ChangeDate = DateTime.Now,
                                UserId = currentUser.Id
                            };
                            currentProduct.DiscountPrice = null;
                            model.AddLog(log);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(DescriptionTextBox.Text))
                        {
                            var log = new AuditLog
                            {
                                ProductId = currentProduct.Id,
                                ActionId = 1, // INSERT
                                FieldId = 4, // DISCOUNT PRICE
                                OldValue = null,
                                ChangeDate = DateTime.Now,
                                UserId = currentUser.Id
                            };
                            currentProduct.Description = DescriptionTextBox.Text;
                            log.NewValue = currentProduct.FormattedDiscountPrice.Replace("₽", "");
                            model.AddLog(log);
                        }
                    }


                    if (currentProduct.Quantity != long.Parse(QuantityTextBox.Text))
                    {
                        var log = new AuditLog
                        {
                            ProductId = currentProduct.Id,
                            ActionId = 3, // UPDATE
                            FieldId = 5, // QUANTITY
                            OldValue = currentProduct.Quantity.ToString(),
                            NewValue = QuantityTextBox.Text,
                            ChangeDate = DateTime.Now,
                            UserId = currentUser.Id
                        };
                        currentProduct.Quantity = long.Parse(QuantityTextBox.Text);
                        model.AddLog(log);
                    }


                    if (currentProduct.CategoryId != ((Categories)CategoryComboBox.SelectedItem).Id)
                    {
                        var log = new AuditLog
                        {
                            ProductId = currentProduct.Id,
                            ActionId = 3, // UPDATE
                            FieldId = 6, // CATEGORY
                            OldValue = currentProduct.CategoryId.ToString(),
                            NewValue = ((Categories)CategoryComboBox.SelectedItem).Id.ToString(),
                            ChangeDate = DateTime.Now,
                            UserId = currentUser.Id
                        };
                        currentProduct.CategoryId = ((Categories)CategoryComboBox.SelectedItem).Id;
                        model.AddLog(log);
                    }

                    var oldProductProperties = model.GetProductPropertiesByProductId(currentProduct.Id);
                        
                    foreach(var pp in oldProductProperties)
                    {
                        model.RemoveProductProperty(pp);
                    }

                    // Добавляем новые свойства
                    foreach(var propertyPair in propertyPairs)
                    {
                        if (string.IsNullOrEmpty(propertyPair.Key.Text) || string.IsNullOrEmpty(propertyPair.Value.Text))
                        {
                            continue; // Пропускаем пустые свойства
                        }
                        
                        
                        var property = new Model.Properties
                        {
                            Name = propertyPair.Key.Text,
                            Value = propertyPair.Value.Text,
                        };
                        model.AddProperty(property);  // Сохраняем, чтобы получить ID

                        var productProperty = new ProductProperty
                        {
                            ProductId = currentProduct.Id,
                            PropertyId = property.Id
                        };
                        model.AddProductProperty(productProperty);
                        
                        // Получаем допустимые значения FieldId из таблицы Fields
                        var validFieldIds = model.SelectFieldsById();
                        
                        // Используем первое доступное значение или 1, если список пуст
                        int propertyFieldId = validFieldIds.Contains(8) ? 8 : (validFieldIds.FirstOrDefault() != 0 ? validFieldIds.FirstOrDefault() : 1);
                        
                        var log = new AuditLog
                        {
                            ProductId = currentProduct.Id,
                            ActionId = 1, // INSERT
                            FieldId = propertyFieldId, // Используем корректный FieldId вместо жестко заданного 8
                            OldValue = null,
                            NewValue = $"{property.Name}:{property.Value}",
                            ChangeDate = DateTime.Now,
                            UserId = currentUser.Id
                        };
                        model.AddLog(log);
                    }
                    MessageBox.Show("Изменения успешно сохранены!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        /// Изменяет отображаемое изображение товара
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
        /// Обработчик получения фокуса полем ввода цены
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void PriceTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MainPriceTextBox.Text == currentProduct.FormattedMainPrice)
            {
                MainPriceTextBox.Text = string.Empty;
                MainPriceTextBox.Foreground = Brushes.Black;
            }
        }

        /// <summary>
        /// Обработчик потери фокуса полем ввода цены
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void PriceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MainPriceTextBox.Text))
            {
                MainPriceTextBox.Text = currentProduct.FormattedMainPrice;
                MainPriceTextBox.Foreground = Brushes.Gray;
            }
        }

        /// <summary>
        /// Обработчик ввода текста в поле основной цены
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void MainPriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var text = MainPriceTextBox.Text + e.Text;
            if (!Regex.IsMatch(text, @"^\d{0,15}(\.\d{0,2})?$"))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обработчик получения фокуса полем ввода скидочной цены
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void DiscountPirceTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DiscountPirceTextBox.Text == currentProduct.FormattedDiscountPrice)
            {
                DiscountPirceTextBox.Text = string.Empty;
                DiscountPirceTextBox.Foreground = Brushes.Black;
            }
        }

        /// <summary>
        /// Обработчик потери фокуса полем ввода скидочной цены
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void DiscountPirceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DiscountPirceTextBox.Text))
            {
                DiscountPirceTextBox.Text = currentProduct.FormattedDiscountPrice;
                DiscountPirceTextBox.Foreground = Brushes.Gray;
            }
        }

        /// <summary>
        /// Обработчик ввода текста в поле скидочной цены
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void DiscountPirceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var text = DiscountPirceTextBox.Text + e.Text;
            if (!Regex.IsMatch(text, @"^\d{0,15}(\.\d{0,2})?$"))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку удаления товара
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы действительно хотите удалить данный продукт?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                model.RemoveProduct(currentProduct);
                MessageBox.Show("Товар удалён!", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }
    }
}
