using System;
using Core.Models;

namespace GroceryStorePOS
{
    public class DiscountManager
    {
        public void DiscountByPercentage(Product product, decimal offPct)
        {
            product.Discounts.Add(new PercentageDiscount { OffPct = offPct });
        }

        public void DiscountByQuantity(Product product, int buyQty, int getFreeQty)
        {
            product.Discounts.Add(new QuantityDiscount { BuyQty = buyQty, GetFreeQty = getFreeQty });
        }
    }
}
