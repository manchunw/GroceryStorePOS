using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Order
    {
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
