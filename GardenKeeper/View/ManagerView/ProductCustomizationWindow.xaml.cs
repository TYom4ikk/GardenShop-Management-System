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

            CategoryComboBox.ItemsSource = Core.context.Categories.ToList();
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

        private void LoadExistingProperties()
        {
            try
            {
                Console.WriteLine("LoadExistingProperties called");
                
                // Получаем все связи товар-свойство для текущего товара
                var productProperties = Core.context.ProductProperty
                    .Where(pp => pp.ProductId == currentProduct.Id)
                    .ToList();

                Console.WriteLine($"Found {productProperties.Count} properties for product {currentProduct.Id}");
                
                if (productProperties.Count > 0)
                {
                    // Используем существующую панель CustomPropertiesStackPanel
                    foreach (var productProperty in productProperties)
                    {
                        // Получаем свойство
                        var property = Core.context.Properties.FirstOrDefault(p => p.Id == productProperty.PropertyId);
                        if (property != null)
                        {
                            Console.WriteLine($"Processing property {property.Id}: {property.Name} = {property.Value}");
                            
                            // Создаем новую панель свойств
                            StackPanel newPropertyPanel = new StackPanel();
                            newPropertyPanel.Orientation = Orientation.Horizontal;
                            newPropertyPanel.Style = (Style)FindResource("ProperyRow");

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
                            Console.WriteLine($"Added to dictionary: {propertyNameTextBox.Text} -> {propertyValueTextBox.Text}");
                            
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
                    
                    Console.WriteLine($"After loading properties, dictionary count: {propertyPairs.Count}");
                    
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
                Console.WriteLine($"Error in LoadExistingProperties: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                
                // В случае ошибки всё равно добавляем пустую строку
                AddEmptyPropertyRow();
            }
        }
        
        // Метод для добавления пустой строки свойства
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
                Console.WriteLine($"Error in AddEmptyPropertyRow: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void UpdateImagesCollection()
        {
            images = Core.context.ProductImages.Where(img => img.ProductId == currentProduct.Id).ToList();
            imageIndex = 0;
        }

        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                Console.WriteLine("AddProperty_Click called");
                
                var currentStackPanel = (sender as Button).Parent as StackPanel;
                var parentStackPanel = currentStackPanel.Parent as StackPanel;

                Console.WriteLine($"Current StackPanel: {currentStackPanel}");
                Console.WriteLine($"Parent StackPanel: {parentStackPanel}");

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
                Console.WriteLine($"Added to dictionary directly: {propertyNameTextBox.Text} -> {propertyValueTextBox.Text}");
                Console.WriteLine($"Dictionary count: {propertyPairs.Count}");
                
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
                Console.WriteLine($"Error in AddProperty_Click: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void PropertyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox nameTextBox = sender as TextBox;
            Console.WriteLine($"PropertyName_TextChanged: {nameTextBox.Text}");
            
            try
            {
                // Получаем родительский контейнер (должен быть StackPanel)
                var nameContainer = nameTextBox.Parent as StackPanel;
                if (nameContainer == null)
                {
                    Console.WriteLine("nameContainer is null");
                    return;
                }
                
                // Получаем контейнер более высокого уровня, содержащий все элементы
                var propertyRow = nameContainer.Parent as StackPanel;
                if (propertyRow == null)
                {
                    Console.WriteLine("propertyRow is null");
                    return;
                }
                
                // Находим контейнер значения (второй StackPanel в ряду)
                StackPanel valueContainer = null;
                TextBox valueTextBox = null;
                
                int childIndex = 0;
                
                // Получаем все дочерние StackPanel
                foreach (var child in propertyRow.Children)
                {
                    Console.WriteLine($"Child {childIndex}: {child.GetType().Name}");
                    
                    if (child is StackPanel && child != nameContainer)
                    {
                        valueContainer = child as StackPanel;
                        break;
                    }
                    childIndex++;
                }
                
                if (valueContainer == null)
                {
                    Console.WriteLine("valueContainer is null");
                    return;
                }
                
                // Находим TextBox в контейнере значения
                foreach (var child in valueContainer.Children)
                {
                    Console.WriteLine($"ValueContainer child: {child.GetType().Name}");
                    
                    if (child is TextBox)
                    {
                        valueTextBox = child as TextBox;
                        break;
                    }
                }
                
                if (valueTextBox == null)
                {
                    Console.WriteLine("valueTextBox is null");
                    return;
                }
                
                // Обновляем словарь
                if (!propertyPairs.ContainsKey(nameTextBox))
                {
                    propertyPairs.Add(nameTextBox, valueTextBox);
                    Console.WriteLine($"Added to dictionary: {nameTextBox.Text} -> {valueTextBox.Text}");
                }
                else
                {
                    propertyPairs[nameTextBox] = valueTextBox;
                    Console.WriteLine($"Updated in dictionary: {nameTextBox.Text} -> {valueTextBox.Text}");
                }
                
                // Обновляем счетчик свойств
                UpdatePropertyCountDisplay();
                
                Console.WriteLine($"Dictionary count: {propertyPairs.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PropertyName_TextChanged: {ex.Message}");
            }
        }

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
                    Console.WriteLine($"Added to dictionary: {nameTextBox.Text} -> {valueTextBox.Text}");
                }
                else
                {
                    propertyPairs[nameTextBox] = valueTextBox;
                    Console.WriteLine($"Updated in dictionary: {nameTextBox.Text} -> {valueTextBox.Text}");
                }
                
                // Обновляем счетчик свойств
                UpdatePropertyCountDisplay();
                
                Console.WriteLine($"Dictionary count: {propertyPairs.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PropertyValue_TextChanged: {ex.Message}");
            }
        }

        // Метод для поиска родительского элемента нужного типа
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
            Core.context.AuditLog.Add(log);
            Core.context.SaveChanges();
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
            Core.context.AuditLog.Add(log);
            Core.context.SaveChanges();
        }

        // Можно оставить пустым (Описание, скидочную цену)
        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Выводим отладочную информацию о свойствах
                Console.WriteLine($"Dictionary count at save: {propertyPairs.Count}");
                
                foreach (var pair in propertyPairs)
                {
                    Console.WriteLine($"Property: {pair.Key.Text} -> {pair.Value.Text}");
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
                        Core.context.AuditLog.Add(log);
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
                                Core.context.AuditLog.Add(log);
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
                            Core.context.AuditLog.Add(log);
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
                            Core.context.AuditLog.Add(log);
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
                        Core.context.AuditLog.Add(log);
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
                                Core.context.AuditLog.Add(log);
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
                            Core.context.AuditLog.Add(log);
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
                            Core.context.AuditLog.Add(log);
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
                        Core.context.AuditLog.Add(log);
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
                        Core.context.AuditLog.Add(log);
                    }

                    // Теперь работаем со свойствами товара
                    Console.WriteLine($"Processing {propertyPairs.Count} property pairs");
                    
                    // Удаляем старые свойства товара
                    var oldProductProperties = Core.context.ProductProperty
                        .Where(pp => pp.ProductId == currentProduct.Id)
                        .ToList();
                        
                    foreach(var pp in oldProductProperties)
                    {
                        Console.WriteLine($"Removing old property: {pp.PropertyId}");
                        Core.context.ProductProperty.Remove(pp);
                    }

                    // Добавляем новые свойства
                    foreach(var propertyPair in propertyPairs)
                    {
                        if (string.IsNullOrEmpty(propertyPair.Key.Text) || string.IsNullOrEmpty(propertyPair.Value.Text))
                        {
                            Console.WriteLine($"Skipping empty property");
                            continue; // Пропускаем пустые свойства
                        }
                        
                        Console.WriteLine($"Adding property: {propertyPair.Key.Text} -> {propertyPair.Value.Text}");
                        
                        var property = new Model.Properties
                        {
                            Name = propertyPair.Key.Text,
                            Value = propertyPair.Value.Text,
                        };
                        Core.context.Properties.Add(property);
                        Core.context.SaveChanges(); // Сохраняем, чтобы получить ID

                        var productProperty = new ProductProperty
                        {
                            ProductId = currentProduct.Id,
                            PropertyId = property.Id
                        };
                        Core.context.ProductProperty.Add(productProperty);
                        
                        // Добавляем лог
                        // Получаем допустимые значения FieldId из таблицы Fields
                        var validFieldIds = Core.context.Fields.Select(f => f.Id).ToList();
                        Console.WriteLine($"Valid Field IDs: {string.Join(", ", validFieldIds)}");
                        
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
                        Core.context.AuditLog.Add(log);
                    }

                    Core.context.SaveChanges();
                    MessageBox.Show("Изменения успешно сохранены!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"Error in SaveChangesButton_Click: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
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

        private void PriceTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MainPriceTextBox.Text == currentProduct.FormattedMainPrice)
            {
                MainPriceTextBox.Text = string.Empty;
                MainPriceTextBox.Foreground = Brushes.Black;
            }
        }

        private void PriceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MainPriceTextBox.Text))
            {
                MainPriceTextBox.Text = currentProduct.FormattedMainPrice;
                MainPriceTextBox.Foreground = Brushes.Gray;
            }
        }

        private void MainPriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var text = MainPriceTextBox.Text + e.Text;
            if (!Regex.IsMatch(text, @"^\d{0,15}(\.\d{0,2})?$"))
            {
                e.Handled = true;
            }
        }

        private void DiscountPirceTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DiscountPirceTextBox.Text == currentProduct.FormattedDiscountPrice)
            {
                DiscountPirceTextBox.Text = string.Empty;
                DiscountPirceTextBox.Foreground = Brushes.Black;
            }
        }

        private void DiscountPirceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DiscountPirceTextBox.Text))
            {
                DiscountPirceTextBox.Text = currentProduct.FormattedDiscountPrice;
                DiscountPirceTextBox.Foreground = Brushes.Gray;
            }
        }

        private void DiscountPirceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var text = DiscountPirceTextBox.Text + e.Text;
            if (!Regex.IsMatch(text, @"^\d{0,15}(\.\d{0,2})?$"))
            {
                e.Handled = true;
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            Core.context.Products.Remove(currentProduct);
            Core.context.SaveChanges();
            MessageBox.Show("Товар удалён!", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        // Метод для обновления отображения количества свойств
        private void UpdatePropertyCountDisplay()
        {
            PropertyPairsCountTextBlock.Text = $"Свойства: {propertyPairs.Count}";
        }
    }
}
