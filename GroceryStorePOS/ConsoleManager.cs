using System;
using System.Collections.Generic;
using Core.Models;

namespace GroceryStorePOS
{
    public class ConsoleManager
    {
        private Order _order;
        public ConsoleManager()
        {
            this._order = new Order();
        }
        public void Scan(string input)
        {
            throw new NotImplementedException();
        }

        public void Cancel(string id)
        {
            throw new NotImplementedException();
        }

        public List<string> Print()
        {
            throw new NotImplementedException();
        }

        public void DiscountByPercentage(string id, double pct)
        {
            throw new NotImplementedException();
        }

        public void DiscountByQuantity(string id, int BuyQty, int GetFreeQty)
        {
            throw new NotImplementedException();
        }

        public Order GetOrder()
        {
            throw new NotImplementedException();
        }
    }
}
