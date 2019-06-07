using System;
using System.Collections.Generic;
using Core.Models;

namespace GroceryStorePOS
{
    public class ConsoleManager
    {
        private Order _order;
        private readonly OrderManager _orderManager;
        private readonly DiscountManager _discountManager;
        public ConsoleManager()
        {
            this._order = new Order();
            this._orderManager = new OrderManager();
            this._discountManager = new DiscountManager();
        }
        public void Scan(string input)
        {
            var inputList = input.Split(",");
            for(var i = 0; i < inputList.Length; i++)
            {
                if (inputList[i] == "Single")
                {
                    i = Increment(inputList, i);
                    this._orderManager.AddProduct(this._order, inputList[i], 1);
                } else if (inputList[i] == "Bulk")
                {
                    i = Increment(inputList, i);
                    var productId = inputList[i];
                    i = Increment(inputList, i);
                    var productQty = int.Parse(inputList[i]);
                    this._orderManager.AddProduct(this._order, productId, productQty);
                } else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Cancel(string id)
        {
            this._orderManager.CancelProduct(this._order, id);
        }

        public List<string> Print()
        {
            return this._orderManager.PrintProducts(this._order);
        }

        public void DiscountByPercentage(string id, decimal offPct)
        {
            var product = this._orderManager.GetProduct(id);
            this._discountManager.DiscountByPercentage(product, offPct);
            this._orderManager.ProcessProduct(product);
        }

        public void DiscountByQuantity(string id, int buyQty, int getFreeQty)
        {
            var product = this._orderManager.GetProduct(id);
            this._discountManager.DiscountByQuantity(product, buyQty, getFreeQty);
            this._orderManager.ProcessProduct(product);
        }

        public Order GetOrder()
        {
            return this._order;
        }

        private int Increment(string[] inputList, int i)
        {
            i++;
            if (i == inputList.Length || string.IsNullOrEmpty(inputList[i]))
            {
                throw new InvalidOperationException();
            }

            return i;
        }
    }
}
