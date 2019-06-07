using System;
using Core.Models.Exception;
using GroceryStorePOS;
using Xunit;

namespace Tests.Common.GroceryStorePOS
{
    public class ConsoleManagerTests
    {
        [Fact]
        public void ScanProductsSuccess()
        {
            var sut = new ConsoleManager();
            sut.Scan("Single,Apple,Bulk,Milk,2");
            var order = sut.GetOrder();
            Assert.Equal(3, order.Products.Count);
            Assert.Equal("Apple", order.Products[0].Id);
            Assert.Equal("Milk", order.Products[1].Id);
            Assert.Equal("Milk", order.Products[2].Id);
        }

        [Fact]
        public void ScanProductsInvalidScanTypeFailure()
        {
            var sut = new ConsoleManager();
            sut.Scan("Invalid");
            Assert.Throws<InvalidOperationException>(() => true);
        }

        [Fact]
        public void ScanProductsInvalidProductFailure()
        {
            var sut = new ConsoleManager();
            sut.Scan("Single,Orange,1");
            Assert.Throws<ProductNotFoundException>(() => true);
        }

        [Fact]
        public void CancelProductSuccess()
        {
            var sut = new ConsoleManager();
            sut.Scan("Single,Apple");
            sut.Cancel("Apple");
            var order = sut.GetOrder();
            Assert.Equal(0, order.Products.Count);
        }

        [Fact]
        public void CancelProductInvalidProductFailure()
        {
            var sut = new ConsoleManager();
            sut.Scan("Single,Apple");
            sut.Cancel("Orange");
            var order = sut.GetOrder();
            Assert.Throws<ProductNotFoundException>(() => true);
        }

        [Fact]
        public void PrintSuccess()
        {
            var sut = new ConsoleManager();
            sut.Scan("Single,Apple,Bulk,Milk,2");
            var ret = sut.Print();
            Assert.Equal("Name Quantity Price Subtotal", ret[0]);
            Assert.Equal("Apple 1 4.0 4.0", ret[1]);
            Assert.Equal("Milk 2 9.41 18.82", ret[2]);
            Assert.Equal("===================", ret[3]);
            Assert.Equal("Total 22.82", ret[4]);
        }

        [Fact]
        public void DiscountByPercentageSuccess()
        {
            var sut = new ConsoleManager();
            sut.DiscountByPercentage("Apple", 0.05);
            sut.Scan("Single,Apple");
            var order = sut.GetOrder();
            Assert.Equal("Apple", order.Products[0].Id);
            Assert.Equal(3.8, order.Products[0].Quantity);
        }

        [Fact]
        public void DiscountByQuantitySuccess()
        {
            var sut = new ConsoleManager();
            sut.DiscountByQuantity("Milk", 2, 1);
            sut.Scan("Bulk,Milk,3");
            var ret = sut.Print();
            Assert.Equal("Name Quantity Price Subtotal", ret[0]);
            Assert.Equal("Milk 3 9.41 18.82", ret[1]);
            Assert.Equal("===================", ret[3]);
            Assert.Equal("Total 18.82", ret[4]);
        }
    }
}
