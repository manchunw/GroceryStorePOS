using System;
using System.Collections.Generic;
using Core.Models;
namespace GroceryStorePOS
{
    public class ProductManager
    {
        private readonly ProductStorage _storage;

        public ProductManager()
        {
            this._storage = new ProductStorage();
        }

        public ProductManager(Dictionary<string, decimal> products)
        {
            this._storage = new ProductStorage(products);
        }

        public List<Product> Get(List<string> ids)
        {
            return this._storage.Get(ids);
        }

        public void SetDiscount(Product product, Discount discount)
        {
            product.Discounts = new List<Discount> { discount };
        }

        public void Process(Product product)
        {
            this._storage.Process(product);
        }
    }
}
