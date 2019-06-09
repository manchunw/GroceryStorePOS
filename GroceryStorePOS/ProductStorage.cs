using System;
using System.Collections.Generic;
using Core.Models;
using Core.Models.Exception;

namespace GroceryStorePOS
{
    public class ProductStorage
    {
        private readonly List<Product> _products = new List<Product>();

        public ProductStorage()
        {
            var factory = new ProductFactory();
            _products.Add(factory.GetProduct("Apple", 4m));
            _products.Add(factory.GetProduct("Milk", 9.41m));
            _products.Add(factory.GetProduct("Eggs (12 pack)", 5.25m));
            _products.Add(factory.GetProduct("Cola (1L)", 12.1m));
            _products.Add(factory.GetProduct("Cola Zero (1L)", 11.15m));
        }

        public List<Product> Get(List<string> ids)
        { 
            return this._products.FindAll(p => ids.Count == 0 || ids.Contains(p.Id));
        }

        public void Process(Product product)
        {
            var idx = this._products.FindIndex((p) => p.Id == product.Id);
            if (idx >= 0)
            {
                this._products[idx] = product;
            }

            // TODO: add product if product is not found
        }
    }
}
