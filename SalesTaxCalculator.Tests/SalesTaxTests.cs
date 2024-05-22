using Xunit;
using SalesTaxCalculator;

namespace SalesTaxApp.Tests
{
    public class SalesTaxTests
    {
        [Fact]
        // Very simple test to ensure Item class works and the price is returned as correct
        public void test1_CheckItemPrice()
        {
            var item = new Item("book", 12.49, false, true);
            double itemTax = ReceiptGenerator.CalculateTax(item);
            double itemTotalPrice = item.Price + itemTax;
            Assert.Equal(12.49, itemTotalPrice, 2); // Precision of 2 decimal places
        }
    }
}