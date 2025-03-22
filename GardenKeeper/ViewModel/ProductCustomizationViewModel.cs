using GardenKeeper.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
