using SalesTaxCalculator;

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

            return RoundUpToNearest5Pence(tax);
        }

        private static double RoundUpToNearest5Pence(double amount)
        {
            // Calculate the nearest 0.05 by multiplying the amount by 20
            // This creates a larger denomination meaning any rounding up will be to the nearest 5 pence
            // and not to the nearest pound.
            // We divide by 20 to revert back to it's correct denomination but with accurate rounding
            return Math.Ceiling(amount * 20) / 20.0;
        }
    }
}
