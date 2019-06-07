using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public List<Discount> Discounts { get; set; } = new List<Discount>();
    }
}
