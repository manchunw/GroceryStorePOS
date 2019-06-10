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

        public OrderManager(ProductManager productManager)
        {
            this._productManager = productManager;
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
            ret.Add($"{nameof(Product.Name)},{nameof(OrderItem.Quantity)},{nameof(Product.Price)},{nameof(OrderItem.Subtotal)},Grand Total");
            decimal grandTotal = 0;
            foreach(var p in order.Items)
            {
                grandTotal += p.Subtotal;
                ret.Add($"{p.Product.Name},{p.Quantity},{p.Product.Price},{p.Subtotal},{grandTotal}");
            }

            ret.Add("===================");
            ret.Add($"Total {order.Total}");
            return ret;
        }

        public void AddProduct(Order order, string id, int qty)
        {
            var product = this._productManager.Get(id);
            order.Items.Add(new OrderItem { Product = product, Quantity = qty });
        }
    }
}
