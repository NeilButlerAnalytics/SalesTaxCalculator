using System;
using System.Collections.Generic;
using System.Globalization;
using SalesTaxApp;
using SalesTaxCalculator;

namespace MyReceiptApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input data
            var inputItems = new List<string>
            {
                "1 book at 12.49",
                "1 music CD at 14.99",
                "1 chocolate bar at 0.85"
            };

            // Parse input items and calculate receipt
            var items = ParseInputItems(inputItems);
            string receipt = ReceiptGenerator.Generate(items);

            // Print receipt to screen
            Console.WriteLine(receipt);
            Console.ReadLine(); // Keep the console open until user intervention
        }

        static List<Item> ParseInputItems(List<string> inputItems)
        {
            var items = new List<Item>();

            foreach (var inputItem in inputItems)
            {
                // Find the index of the first space
                int spaceIndex = inputItem.IndexOf(' ');

                // Extract the quantity substring
                int quantity = int.Parse(inputItem.Substring(0, spaceIndex));

                // Extract the item details substring
                string itemDetails = inputItem.Substring(spaceIndex + 1);

                // Determine if the item is imported
                bool isImported = itemDetails.Contains("imported");

                // Determine if the item is exempt from tax
                bool isExempt = itemDetails.Contains("book") || itemDetails.Contains("chocolate") || itemDetails.Contains("paracetamol");

                // Find the index of "at" to separate the item name and price
                int atIndex = itemDetails.LastIndexOf(" at ");
                string itemName = itemDetails.Substring(0, atIndex).Replace("imported ", "").Trim();
                double price = double.Parse(itemDetails.Substring(atIndex + 4));

                // Calculate the total price for the current item
                double totalPrice = price * quantity;

                // Create an Item object and add it to the list
                items.Add(new Item(itemName, totalPrice, isImported, isExempt, quantity));
            }

            return items;
        }
    }
}
