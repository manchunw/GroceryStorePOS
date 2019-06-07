using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models
{
    public class Order
    {
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal Total => this.Items.Select(p => p.Subtotal).Sum();
    }
}
