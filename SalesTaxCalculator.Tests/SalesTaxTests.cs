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
        // Test to use Theory, check multiple inputs - ALL SHOULD BE SUCCESSFUL
        [Theory]
        [InlineData("book", 12.49, false, true, 12.49)]
        [InlineData("music CD", 14.99, false, false, 16.49)]
        [InlineData("chocolate bar", 0.85, false, true, 0.85)]
        [InlineData("imported box of chocolates", 10.00, true, true, 10.50)]
        [InlineData("imported bottle of perfume", 47.50, true, false, 54.65)]
        [InlineData("imported bottle of perfume", 27.99, true, false, 32.19)]
        [InlineData("bottle of perfume", 18.99, false, false, 20.89)]
        [InlineData("packet of headache pills", 9.75, false, true, 9.75)]
        [InlineData("imported box of chocolates", 11.25, true, true, 11.85)]
        public void test2_CheckItemPriceMultiple(string name, double price, bool isImported, bool isExempt, double expectedPrice)
        {
            var item = new Item(name, price, isImported, isExempt);
            double itemTax = ReceiptGenerator.CalculateTax(item);
            double itemTotalPrice = item.Price + itemTax;
            Assert.Equal(expectedPrice, itemTotalPrice, 2);
        }

        // Same Test as above but this time three items are purposefully incorrect, these tests should FAIL
        [Theory]
        [InlineData("book", 12.49, false, true, 12.49)]
        [InlineData("music CD", 14.99, false, false, 16.49)]
        [InlineData("chocolate bar", 0.85, false, true, 0.85)]
        [InlineData("imported box of chocolates", 10.00, true, true, 10.51)] // INCORRECT
        [InlineData("imported bottle of perfume", 47.50, true, false, 54.68)] // INCORRECT
        [InlineData("imported bottle of perfume", 27.99, true, false, 32.19)]
        [InlineData("bottle of perfume", 18.99, false, false, 20.89)]
        [InlineData("packet of headache pills", 9.75, false, true, 9.00)] // INCORRECT
        [InlineData("imported box of chocolates", 11.25, true, true, 11.85)]
        public void test3_CheckItemPriceMultiple(string name, double price, bool isImported, bool isExempt, double expectedPrice)
        {
            var item = new Item(name, price, isImported, isExempt);
            double itemTax = ReceiptGenerator.CalculateTax(item);
            double itemTotalPrice = item.Price + itemTax;
            Assert.Equal(expectedPrice, itemTotalPrice, 2);
        }
        // Generate method created in Receipt.cs - this method creates a receipt as string based on itmes and price and total price after tax
        // This first test is based on the first required output in the specification
        [Fact]
        public void test4_GenerateAReceipt()
        {
            var items = new List<Item>
            {
                new Item("book", 12.49, false, true),
                new Item("music CD", 14.99, false, false),
                new Item("chocolate bar", 0.85, false, true)
            };
            var expectedReceipt = "book: 12.49\nmusic CD: 16.49\nchocolate bar: 0.85\nSales Taxes: 1.50\nTotal: 29.83";
            Assert.Equal(expectedReceipt, ReceiptGenerator.Generate(items));
        }
    }
}