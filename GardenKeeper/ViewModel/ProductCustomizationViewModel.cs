using GardenKeeper.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace GardenKeeper.ViewModel
{
    public class ProductCustomizationViewModel
    {
        public List<Model.Properties> properties;

        /// <summary>
        /// Инициализирует новый экземпляр класса ProductCustomizationViewModel и загружает все свойства
        /// </summary>
        public ProductCustomizationViewModel()
        {
            properties = Core.context.Properties.ToList();
        }

        /// <summary>
        /// Добавляет новое свойство
        /// </summary>
        /// <param name="name">Название свойства</param>
        /// <param name="value">Значение свойства</param>
        public void AddNewProperty(string name, string value)
        {
            Model.Properties property = new Model.Properties { Name = name, Value = value};
        }

        /// <summary>
        /// Получает список всех категорий
        /// </summary>
        /// <returns>Список категорий</returns>
        public List<Categories> GetCategories()
        {
            return Core.context.Categories.ToList();
        }

        /// <summary>
        /// Получает список свойств товара по его идентификатору
        /// </summary>
        /// <param name="productId">Идентификатор товара</param>
        /// <returns>Список свойств товара</returns>
        public List<ProductProperty> GetProductPropertiesByProductId(int productId)
        {
            return Core.context.ProductProperty.Where(pp => pp.ProductId == productId)
                    .ToList();
        }

        /// <summary>
        /// Удаляет свойство товара
        /// </summary>
        /// <param name="productProperty">Свойство товара для удаления</param>
        public void RemoveProductProperty(ProductProperty productProperty)
        {
            Core.context.ProductProperty.Remove(productProperty);
        }

        /// <summary>
        /// Удаляет товар
        /// </summary>
        /// <param name="product">Товар для удаления</param>
        public bool RemoveProduct(Products product)
        {
            if (Core.context.Products.Any(p => p.Id == product.Id))
            {

                Core.context.Products.Remove(product);
                Core.context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// Добавляет новое свойство в базу данных
        /// </summary>
        /// <param name="property">Свойство для добавления</param>
        public void AddProperty(Model.Properties property)
        {
            Core.context.Properties.Add(property);
            Core.context.SaveChanges();
        }

        /// <summary>
        /// Добавляет связь между товаром и свойством
        /// </summary>
        /// <param name="productProperty">Связь для добавления</param>
        public void AddProductProperty(Model.ProductProperty productProperty)
        {
            Core.context.ProductProperty.Add(productProperty);
            Core.context.SaveChanges();
        }

        /// <summary>
        /// Получает свойство по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор свойства</param>
        /// <returns>Объект свойства или null, если свойство не найдено</returns>
        public Model.Properties GetPropertyById(long? id)
        {
            return Core.context.Properties.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Получает список изображений товара по его идентификатору
        /// </summary>
        /// <param name="productId">Идентификатор товара</param>
        /// <returns>Список изображений товара</returns>
        public List<ProductImages> GetProductImagesByProductId(int productId)
        {
            return Core.context.ProductImages.Where(pp => pp.ProductId == productId)
                    .ToList();
        }

        /// <summary>
        /// Получает список идентификаторов полей
        /// </summary>
        /// <returns>Список идентификаторов полей</returns>
        public List<int> SelectFieldsById()
        {
            return Core.context.Fields.Select(f => f.Id).ToList();
        }

        /// <summary>
        /// Добавляет запись в журнал аудита
        /// </summary>
        /// <param name="log">Запись для добавления</param>
        public void AddLog(AuditLog log)
        {
            Core.context.AuditLog.Add(log);
            Core.context.SaveChanges();
        }

        /// <summary>
        /// Добавляет новое изображение для товара
        /// </summary>
        /// <param name="productId">Идентификатор товара</param>
        /// <returns>true, если изображение успешно добавлено, иначе false</returns>
        public bool AddNewImage(int productId)
        {
            try
            {
                if(!Core.context.Products.Any(p => p.Id == productId))
                {
                    MessageBox.Show("Попробуйте сначала создать товар, а затем изменять картинки!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                OpenFileDialog openFileDialog = new OpenFileDialog();
                var result = openFileDialog.ShowDialog();
                if (result == false)
                {
                    return false;
                }
                byte[] image_bytes = System.IO.File.ReadAllBytes(openFileDialog.FileName);

                var images = Core.context.ProductImages;

                images.Add(new ProductImages { Image = image_bytes, ProductId = productId });
                Core.context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        /// <summary>
        /// Удаляет изображение по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор изображения</param>
        /// <returns>true, если изображение успешно удалено, иначе false</returns>
        public bool DeleteImage(long id)
        {
            try
            {
                var image = Core.context.ProductImages.FirstOrDefault(x => x.Id == id);
                Core.context.ProductImages.Remove(image);
                Core.context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет, существует ли продукт в таблице Products
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true, если существует, иначе false</returns>
        public bool ContainsProductById(int id)
        {
            return Core.context.Products.Any(p => p.Id == id);
        }

        /// <summary>
        /// Добавляет продукт в Базу данных
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Products product)
        {
            Core.context.Products.Add(product);
            Core.context.SaveChanges();
        }

        /// <summary>
        /// Получает последний продукт из базы данных, отсортированный по убыванию идентификатора.
        /// </summary>
        /// <returns>Последний добавленный продукт</returns>
        public Products GetLastProduct()
        {
            return Core.context.Products.OrderByDescending(p => p.Id).FirstOrDefault();
        }
    }
}
