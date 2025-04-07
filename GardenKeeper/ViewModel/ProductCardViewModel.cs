using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardenKeeper.Model;

namespace GardenKeeper.ViewModel
{
    internal class ProductCardViewModel
    {
        public List<ProductImages> GetProductImagesByProductId(int productId)
        {
            return Core.context.ProductImages.Where(pp => pp.ProductId == productId)
                    .ToList();
        }
    }
}
