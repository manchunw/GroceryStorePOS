using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Models.Exception;

namespace GroceryStorePOS
{
    public class ProductManager
    {
        private readonly ProductStorage _storage;

        public ProductManager(ProductStorage productStorage)
        {
            this._storage = productStorage;
        }

        public List<Product> Get(List<string> ids)
        {
            return this._storage.Get(ids);
        }

        public Product Get(string id)
        {
            var ret = this.Get(new List<string> { id }).FirstOrDefault();
            if (ret == null)
            {
                throw new ProductNotFoundException();
            }

            return ret;
        }

        public void Process(Product product)
        {
            this._storage.Process(product);
        }
    }
}
