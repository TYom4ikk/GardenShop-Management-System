using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardenKeeper.Model;

namespace GardenKeeper.ViewModel
{
    /// <summary>
    /// ViewModel для окна отображения полной информации о товаре
    /// </summary>
    public class ProductFullInfoViewModel
    {
        /// <summary>
        /// Получает список изображений товара по его идентификатору
        /// </summary>
        /// <param name="productId">Идентификатор товара</param>
        /// <returns>Список изображений товара</returns>
        public List<ProductImages> GetImagesByProductId(int productId)
        {
            return Core.context.ProductImages.Where(img => img.ProductId == productId).ToList();
        }

        /// <summary>
        /// Получает список свойств товара по его идентификатору
        /// </summary>
        /// <param name="productId">Идентификатор товара</param>
        /// <returns>Список свойств товара</returns>
        public List<ProductProperty> GetProductPropertyByProductId(int productId)
        {
            return Core.context.ProductProperty
                .Where(pp => pp.ProductId == productId)
                .ToList();
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
    }
}
