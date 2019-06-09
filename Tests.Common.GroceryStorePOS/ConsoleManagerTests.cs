using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using Core.Models.Exception;
using GroceryStorePOS;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;

namespace Tests.Common.GroceryStorePOS
{
    [ExcludeFromCodeCoverage]
    public class ConsoleManagerTests
    {
        private readonly IFixture _fixture;
        public ConsoleManagerTests()
        {
            this._fixture = new Fixture();
        }

        [Fact]
        public void ScanProductsSuccess()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            sut.Scan("Single,Apple,Bulk,Milk,2");
            var order = sut.GetOrder();

            Assert.Equal(2, order.Items.Count);
            Assert.Equal("Apple", order.Items[0].Product.Id);
            Assert.Equal(1, order.Items[0].Quantity);
            Assert.Equal("Milk", order.Items[1].Product.Id);
            Assert.Equal(2, order.Items[1].Quantity);
        }

        [Fact]
        public void ScanProductsInsufficientArgumentFailure()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            Assert.Throws<InvalidOperationException>(() => sut.Scan("Single,Apple,Bulk"));
        }

        [Fact]
        public void ScanProductsNegativeQuantityFailure()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            Assert.Throws<InvalidQuantityException>(() => sut.Scan("Single,Apple,Bulk,Milk,-1"));
        }

        [Fact]
        public void ScanProductsZeroQuantityFailure()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            Assert.Throws<InvalidQuantityException>(() => sut.Scan("Single,Apple,Bulk,Milk,0"));
        }

        [Fact]
        public void ScanProductsInvalidScanTypeFailure()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            Assert.Throws<InvalidOperationException>(() => sut.Scan("Invalid"));
        }

        [Fact]
        public void ScanProductsInvalidProductFailure()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            Assert.Throws<ProductNotFoundException>(() => sut.Scan("Single,Orange"));
        }

        [Fact]
        public void CancelSingleProductSuccess()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            sut.Scan("Single,Apple");
            var order = sut.GetOrder();

            Assert.Equal("Apple", order.Items[0].Product.Id);

            sut.Cancel("Apple");
            order = sut.GetOrder();

            Assert.Empty(order.Items);
        }

        [Fact]
        public void CancelBulkProductSuccess()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            sut.Scan("Bulk,Apple,2");
            var order = sut.GetOrder();

            Assert.Equal("Apple", order.Items[0].Product.Id);
            Assert.Equal(2, order.Items[0].Quantity);

            sut.Cancel("Apple");
            order = sut.GetOrder();

            Assert.Equal("Apple", order.Items[0].Product.Id);
            Assert.Equal(1, order.Items[0].Quantity);

            sut.Cancel("Apple");
            order = sut.GetOrder();

            Assert.Empty(order.Items);
        }

        [Fact]
        public void CancelProductInvalidProductFailure()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            Assert.Throws<ProductNotFoundException>(() => sut.Cancel("Orange"));
        }

        [Fact]
        public void CancelBulkProductNegativeQuantitySuccess()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            Assert.Throws<InvalidQuantityException>(() => sut.Scan("Bulk,Apple,-2"));
        }

        [Fact]
        public void CancelBulkProductInvalidQuantitySuccess()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            Assert.Throws<InvalidQuantityException>(() => sut.Scan("Bulk,Apple,3.6"));
        }

        [Fact]
        public void PrintEmptyListSuccess()
        {
            var sut = this._fixture.Create<ConsoleManager>();
            var ret = sut.Print();

            Assert.Equal("Name,Quantity,Price,Subtotal,Grand Total", ret[0]);
            Assert.Equal("===================", ret[1]);
            Assert.Equal("Total 0", ret[2]);
        }


        [Fact]
        public void PrintSuccess()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            sut.Scan("Single,Apple,Bulk,Milk,2");
            var ret = sut.Print();

            Assert.Equal("Name,Quantity,Price,Subtotal,Grand Total", ret[0]);
            Assert.Equal("Apple,1,4,4,4", ret[1]);
            Assert.Equal("Milk,2,9.41,18.82,22.82", ret[2]);
            Assert.Equal("===================", ret[3]);
            Assert.Equal("Total 22.82", ret[4]);
        }

        [Fact]
        public void DiscountByPercentageSuccess()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            sut.DiscountByPercentage("Apple", 0.05m);
            sut.Scan("Single,Apple");
            var ret = sut.GetOrder();

            Assert.Equal("Apple", ret.Items[0].Product.Id);
            Assert.Equal(3.8m, ret.Items[0].Product.Price);
        }

        [Fact]
        public void DiscountByQuantitySuccess()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            sut.DiscountByQuantity("Milk", 2, 1);
            sut.Scan("Bulk,Milk,3");
            var ret = sut.GetOrder();

            Assert.Equal("Milk", ret.Items[0].Product.Id);
            Assert.Equal(9.41m, ret.Items[0].Product.Price);
            Assert.Equal(18.82m, ret.Total);
        }

        [Fact]
        public void DiscountByPercentageInvalidProductFailure()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            Assert.Throws<ProductNotFoundException>(() => sut.DiscountByPercentage("Orange", 0.05m));
        }

        [Fact]
        public void DiscountByQuantityInvalidProductFailure()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            Assert.Throws<ProductNotFoundException>(() => sut.DiscountByQuantity("Orange", 2, 1));
        }

        [Fact]
        public void DiscountByPercentagePrintMessageSuccess()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            sut.DiscountByPercentage("Apple", 0.05m);
            sut.Scan("Single,Apple");
            var ret = sut.Print();

            Assert.Equal("Name,Quantity,Price,Subtotal,Grand Total", ret[0]);
            Assert.Equal("Apple,1,3.80,3.80,3.80", ret[1]);
            Assert.Equal("===================", ret[2]);
            Assert.Equal("Total 3.80", ret[3]);
        }

        [Fact]
        public void DiscountByQuantityPrintMessageSuccess()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            sut.DiscountByQuantity("Milk", 2, 1);
            sut.Scan("Bulk,Milk,3");
            var ret = sut.Print();

            Assert.Equal("Name,Quantity,Price,Subtotal,Grand Total", ret[0]);
            Assert.Equal("Milk,3,9.41,18.82,18.82", ret[1]);
            Assert.Equal("===================", ret[2]);
            Assert.Equal("Total 18.82", ret[3]);
        }

        [Fact]
        public void ClearOrderSuccess()
        {
            var sut = this._fixture.Create<ConsoleManager>();

            sut.Scan("Bulk,Milk,3");
            sut.ClearOrder();
            var ret = sut.GetOrder();

            Assert.Empty(ret.Items);
        }
    }
}
