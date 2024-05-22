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
            var inputItems1 = new List<string>
            {
                "1 book at 12.49",
                "1 music CD at 14.99",
                "1 chocolate bar at 0.85"
            };

            var inputItems2 = new List<string>
            {
                "1 Imported box of chocolates at 10.00",
                "1 Imported bottle of perfume at 47.50",
            };

            var inputItems3 = new List<string>
            {
                "1 Imported bottle of perfume at 27.99",
                "1 Bottle of perfume at 18.99",
                "1 Packet of paracetamol at 9.75",
                "1 Box of imported chocolates at 11.25"
            };

            var inputItems4 = new List<string>
            {
                "2 Imported bottle of perfume at 27.99",
                "12 Bottle of perfume at 18.99",
                "5 Packet of paracetamol at 9.75",
                "4 Box of imported chocolates at 11.25",
                "58 chocolate bar at 0.85",
                "6 book at 12.49",
                "4 music CD at 14.99"
            };
            string receipt = "";
            // Parse input items and calculate receipt
            if (inputItems1.Count != 0)
            {
                var items1 = ParseInputItems(inputItems1);
                receipt = ReceiptGenerator.Generate(items1);
            }

            if (inputItems2.Count != 0)
            {
                var items2 = ParseInputItems(inputItems2);
                receipt += "\n\n" + ReceiptGenerator.Generate(items2);
            }
            if (inputItems3.Count != 0)
            {
                var items3 = ParseInputItems(inputItems3);
                receipt += "\n\n" + ReceiptGenerator.Generate(items3);
            }
            // Extra input to show code working with multiple items.
            if (inputItems4.Count != 0)
            {
                var items4 = ParseInputItems(inputItems4);
                receipt += "\n\n----------------------------\n\n" + ReceiptGenerator.Generate(items4);
            }
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
                string itemDetails = (inputItem.Substring(spaceIndex + 1)).ToLower();

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
