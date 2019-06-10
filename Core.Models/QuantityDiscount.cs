using System;
namespace Core.Models
{
    public class QuantityDiscount : Discount
    {
        public int BuyQty { get; set; }
        public int GetFreeQty { get; set; }
    }
}
