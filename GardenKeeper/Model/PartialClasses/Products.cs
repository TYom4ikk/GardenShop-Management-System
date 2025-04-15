using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenKeeper.Model
{
    public partial class Products
    {
        public string FormattedMainPrice
        {
            get
            {
                string mainPriceStr = MainPrice.ToString();

                string rubles = mainPriceStr.Substring(0, mainPriceStr.Length - 2);
                string kopecks = mainPriceStr.Substring(mainPriceStr.Length - 2);
                return $"{rubles}.{kopecks} ₽";
            }
        }
        public string FormattedDiscountPrice
        {
            get
            {
                if (DiscountPrice == null)
                {
                    return "";
                }
                string discountPriceStr = DiscountPrice.ToString();

                string rubles = discountPriceStr.Substring(0, discountPriceStr.Length - 2);
                string kopecks = discountPriceStr.Substring(discountPriceStr.Length - 2);
                return $"{rubles}.{kopecks} ₽";
            }
        }
        public Nullable<long> PriceToSort
        {
            get
            {
                if (DiscountPrice == null)
                {
                    return MainPrice;
                }
                return DiscountPrice;
            }
            set
            {
            }
        }
        public long SelectedQuantity = 0;
        public byte[] MainImage { get; set; }
    }
}
