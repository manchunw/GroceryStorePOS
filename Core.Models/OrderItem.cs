using System;
namespace Core.Models
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal
        {
            get
            {
                var ret = this.Quantity * this.Product.Price;
                foreach (var d in this.Product.Discounts)
                {
                    if (d is QuantityDiscount qtyDiscount)
                    {
                        if (this.Quantity % (qtyDiscount.BuyQty + qtyDiscount.GetFreeQty) == 0)
                        {
                            ret = (this.Quantity / (qtyDiscount.BuyQty + qtyDiscount.GetFreeQty) * qtyDiscount.BuyQty) * this.Product.Price;
                        }
                    }
                }
                return ret;
            }
        }
    }
}
