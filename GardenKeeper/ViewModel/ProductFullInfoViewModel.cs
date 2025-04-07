using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardenKeeper.Model;

namespace GardenKeeper.ViewModel
{
    internal class ProductFullInfoViewModel
    {
        public List<ProductImages> GetImagesByProductId(int productId)
        {
            return Core.context.ProductImages.Where(img => img.ProductId == productId).ToList();
        }

        public List<ProductProperty> GetProductPropertyByProductId(int productId)
        {
            return Core.context.ProductProperty
                .Where(pp => pp.ProductId == productId)
                .ToList();
        }

        public Model.Properties GetPropertyById(long? id)
        {
            return Core.context.Properties.FirstOrDefault(p => p.Id == id);
        }
    }
}
