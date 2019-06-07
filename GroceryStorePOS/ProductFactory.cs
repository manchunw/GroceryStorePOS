using System;
using Core.Models;

namespace GroceryStorePOS
{
    public class ProductFactory
    {
        public Product GetProduct(string name, decimal price)
        {
            return new Product();
        }
    }
}
