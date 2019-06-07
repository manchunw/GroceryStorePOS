using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Models.Exception;

namespace GroceryStorePOS
{
    public class OrderManager
    {
        private readonly ProductManager _productManager;
        public OrderManager()
        {
            this._productManager = new ProductManager();
        }

        public decimal GetTotalPrice(Order order)
        {
            return order.Total;
        }

        public void CancelProduct(Order order, string id)
        {
            var idx = order.Items.FindIndex((o) => o.Product.Id == id);
            if (idx < 0)
            {
                throw new ProductNotFoundException();
            }

            order.Items[idx].Quantity -= 1;
            if (order.Items[idx].Quantity == 0)
            {
                order.Items.RemoveAt(idx);
            }
        }

        public List<string> PrintProducts(Order order)
        {
            var ret = new List<string>();
            ret.Add($"{nameof(Product.Name)} {nameof(OrderItem.Quantity)} {nameof(Product.Price)} {nameof(OrderItem.Subtotal)}");
            foreach(var p in order.Items)
            {
                ret.Add($"{p.Product.Name} {p.Quantity} {p.Product.Price} {p.Subtotal}");
            }

            ret.Add("===================");
            ret.Add($"Total {order.Total}");
            return ret;
        }

        public void AddProduct(Order order, string id, int qty)
        {
            var product = this.GetProduct(id);
            order.Items.Add(new OrderItem { Product = product, Quantity = qty });
        }

        public Product GetProduct(string id)
        {
            var ret = this._productManager.Get(new List<string> { id }).FirstOrDefault();
            if (ret == null)
            {
                throw new ProductNotFoundException();
            }

            return ret;
        }

        public void ProcessProduct(Product product)
        {
            this._productManager.Process(product);
        }
    }
}
