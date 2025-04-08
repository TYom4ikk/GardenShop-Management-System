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
    }
}
