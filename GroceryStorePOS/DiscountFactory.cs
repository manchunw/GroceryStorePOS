using System;
using Core.Models;

namespace GroceryStorePOS
{
    public class DiscountFactory
    {
        public PercentageDiscount GetDiscountByPercentage(decimal offPct)
        {
            return new PercentageDiscount { OffPct = offPct };
        }

        public QuantityDiscount GetDiscountByQuantity(int buyQty, int getFreeQty)
        {
            return new QuantityDiscount { BuyQty = buyQty, GetFreeQty = getFreeQty };
        }
    }
}
