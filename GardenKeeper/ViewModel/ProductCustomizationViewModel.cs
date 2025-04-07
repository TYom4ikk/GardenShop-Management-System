using GardenKeeper.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace GardenKeeper.ViewModel
{
    public class ProductCustomizationViewModel
    {
        public List<Model.Properties> properties;
        public ProductCustomizationViewModel()
        {
            properties = Core.context.Properties.ToList();
        }
        public void AddNewProperty(string name, string value)
        {
            Model.Properties property = new Model.Properties { Name = name, Value = value};
        }

        public List<Categories> GetCategories()
        {
            return Core.context.Categories.ToList();
        }

        public List<ProductProperty> GetProductPropertiesByProductId(int productId)
        {
            return Core.context.ProductProperty.Where(pp => pp.ProductId == productId)
                    .ToList();
        }

        public void RemoveProductProperty(ProductProperty productProperty)
        {
            Core.context.ProductProperty.Remove(productProperty);
        }

        public void RemoveProduct(Products product)
        {
            Core.context.Products.Remove(product);
            Core.context.SaveChanges();
        }

        public void AddProperty(Model.Properties property)
        {
            Core.context.Properties.Add(property);
            Core.context.SaveChanges();
        }

        public void AddProductProperty(Model.ProductProperty productProperty)
        {
            Core.context.ProductProperty.Add(productProperty);
            Core.context.SaveChanges();
        }

        public Model.Properties GetPropertyById(long? id)
        {
            return Core.context.Properties.FirstOrDefault(p => p.Id == id);
        }

        public List<ProductImages> GetProductImagesByProductId(int productId)
        {
            return Core.context.ProductImages.Where(pp => pp.ProductId == productId)
                    .ToList();
        }

        public List<int> SelectFieldsById()
        {
            return Core.context.Fields.Select(f => f.Id).ToList();
        }

        public void AddLog(AuditLog log)
        {
            Core.context.AuditLog.Add(log);
            Core.context.SaveChanges();
        }

        public bool AddNewImage(int productId)
        {
            try
            {
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
    }
}
