using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        private decimal PriceRaw { get; set; }
        public decimal Price
        {
            get
            {
                var ret = this.PriceRaw;
                foreach (var d in Discounts)
                {
                    if (d is PercentageDiscount pctDiscount)
                    {
                        ret = ret * (1 - pctDiscount.OffPct);
                    }
                }
                return ret;
            }
            set { this.PriceRaw = value; }
        }
        public List<Discount> Discounts { get; set; } = new List<Discount>();
    }
}
