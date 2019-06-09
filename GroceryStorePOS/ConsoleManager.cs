using System;
using System.Collections.Generic;
using Core.Models;
using Core.Models.Exception;

namespace GroceryStorePOS
{
    public class ConsoleManager
    {
        private readonly Order _order;
        private readonly OrderManager _orderManager;
        private readonly DiscountFactory _discountFactory;

        public ConsoleManager(OrderManager orderManager, DiscountFactory discountFactory)
        {
            this._order = new Order();
            this._orderManager = orderManager;
            this._discountFactory = discountFactory;
        }

        public void Scan(string input)
        {
            var inputList = input.Split(",");
            for(var i = 0; i < inputList.Length; i++)
            {
                switch (inputList[i])
                {
                    case "Single":
                        i = Increment(inputList, i);
                        this._orderManager.AddProduct(this._order, inputList[i], 1);
                        break;
                    case "Bulk":
                        i = Increment(inputList, i);
                        var productId = inputList[i];
                        i = Increment(inputList, i);
                        if (int.TryParse(inputList[i], out int productQty))
                        {
                            if (productQty <= 0)
                            {
                                throw new InvalidQuantityException($"Product quantity must be a positive integer: {inputList[i]}");
                            }
                            this._orderManager.AddProduct(this._order, productId, productQty);
                        } else
                        {
                            throw new InvalidQuantityException($"Product quantity cannot be parsed as int at index {i}");
                        }

                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected command at index {i}: {inputList[i]}");
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
            var discount = this._discountFactory.GetDiscountByPercentage(offPct);
            product.Discounts = new List<Discount> { discount };
            this._orderManager.ProcessProduct(product);
        }

        public void DiscountByQuantity(string id, int buyQty, int getFreeQty)
        {
            var product = this._orderManager.GetProduct(id);
            var discount = this._discountFactory.GetDiscountByQuantity(buyQty, getFreeQty);
            product.Discounts = new List<Discount> { discount };
            this._orderManager.ProcessProduct(product);
        }

        public void ClearOrder()
        {
            this._order.Items.Clear();
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
