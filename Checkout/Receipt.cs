using SalesTaxCalculator;
using System.Globalization;

namespace SalesTaxApp
{
    public static class ReceiptGenerator
    {
        // Method takes an item and returns a decimal representing the tax amount
         public static double CalculateTax(Item item)
        {
            double tax = 0;
            // Check if the item is not exempt from basic tax
            if (!item.IsExempt)
            {
                // If the item is not exempt, calculate the basic tax (10%) and add it to the tax amount
                tax += item.Price * 0.10;
            }
            // Check if the item is imported
            if (item.IsImported)
            {
                // If the item is imported, calculate the import duty (5%) and add it to the tax amount
                tax += item.Price * 0.05;
            }
            var test = RoundUpToNearest5Pence(tax);
            return test;
        }

        private static double RoundUpToNearest5Pence(double amount)
        {
            // Calculate the nearest 0.05 by multiplying the amount by 20
            // This creates a larger denomination meaning any rounding up will be to the nearest 5 pence
            // and not to the nearest pound.
            // We divide by 20 to revert back to it's correct denomination but with accurate rounding
            var test = Math.Ceiling(amount * 20);
            var test2 = test / 20.0;
            return test2;
        }

        // Method that generates a receipt string based on a list of items
        public static string Generate(List<Item> items)
        {
            double totalTax = 0;
            double totalPrice = 0;
            List<string> receiptLines = new List<string>();

            foreach (var item in items)
            {
                double test = CalculateTax(item);
                int quantity = item.Quantity;
                // double itemTax = CalculateTax(item) * item.Quantity; // Multiply tax by quantity
                double itemTax = CalculateTax(item);
                double itemTotalPrice = item.Price + itemTax;
                

                totalTax += itemTax;
                totalPrice += itemTotalPrice;

                receiptLines.Add($"{item.Quantity} {item.Name}: {itemTotalPrice.ToString("0.00")}");
            }

            receiptLines.Add($"Sales Taxes: {totalTax.ToString("0.00")}");
            receiptLines.Add($"Total: {totalPrice.ToString("0.00")}");

            return string.Join("\n", receiptLines);
        }

    }
}
