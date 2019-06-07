﻿using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<Discount> Discounts { get; set; } = new List<Discount>();
    }
}
