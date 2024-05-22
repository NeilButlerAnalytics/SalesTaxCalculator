using SalesTaxApp;
using SalesTaxCalculator;

namespace MyReceiptApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath;
            // Check if a file path is provided as a command-line argument
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide the path to the CSV file");
                filePath = Console.ReadLine();
            }
            else
            {
                filePath = args[0];
            }

            // Read items from CSV if the file exists
            List<string> inputItems;
            if (File.Exists(filePath))
            {
                inputItems = ReadItemsFromCSV(filePath); // Read items from CSV
            }
            else
            {
                Console.WriteLine("CSV file not found!"); // Print error message if file not found
                return;
            }

            string receipt = ""; // Initialize receipt string
            string currentTitle = ""; // Store the current title

            List<List<string>> allInputIterations = new List<List<string>>(); // Initialize list for all input iterations

            // Loop through input items to separate different input iterations
            foreach (var items in inputItems)
            {
                // Check if the line is empty or not
                if (string.IsNullOrWhiteSpace(items))
                {
                    currentTitle = ""; // Reset title on empty line
                    continue;
                }

                // Check if the line contains "Input"
                if (!items.Contains("Input"))
                {
                    // Add items to the current input iteration list
                    allInputIterations[allInputIterations.Count - 1].Add(items);
                }
                else
                {
                    // "Input" detected, create a new input iteration list
                    allInputIterations.Add(new List<string>());
                    // Handle new input title or other initial processing if needed
                    // currentTitle = items; // Example of handling the new input title
                }
            }

            // Process each input iteration separately
            foreach (var iteration in allInputIterations)
            {
                // Parse items and generate receipt for this iteration
                var parsedItems = ParseInputItems(iteration);
                receipt += currentTitle + "\n" + ReceiptGenerator.Generate(parsedItems) + "\n\n";
            }

            // Print receipt to screen
            Console.WriteLine(receipt);
            Console.ReadLine(); // Keep the console open until user intervention
        }

        // Method to read items from CSV file
        static List<string> ReadItemsFromCSV(string filePath)
        {
            var items = new List<string>();
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    items.Add(reader.ReadLine());
                }
            }
            return items;
        }

        // Method to parse input items and generate list of Item objects
        static List<Item> ParseInputItems(List<string> inputItems)
        {
            var items = new List<Item>();

            foreach (var inputItem in inputItems)
            {
                if (!inputItem.Contains("Input"))
                {
                    // Find the index of the last occurrence of " at " to split the string
                    int atIndex = inputItem.LastIndexOf(" at ");

                    // Ensure " at " is found and it's not the last character in the string
                    if (atIndex >= 0 && atIndex < inputItem.Length - 1)
                    {
                        // Extract the quantity, item name, and price from the input string
                        
                        int firstSpaceIndex = inputItem.IndexOf(' ');// Extract the quantity from the beginning of the string up to the first space
                        string quantityStr = inputItem.Substring(0, firstSpaceIndex).Trim();
                        string itemName = (inputItem.Substring(quantityStr.Length + 1, atIndex - quantityStr.Length - 1).Trim()).ToLower();
                        string priceStr = inputItem.Substring(atIndex + 4).Trim();

                        // Parse quantity and price
                        if (int.TryParse(quantityStr, out int quantity) && double.TryParse(priceStr, out double price))
                        {
                            // Determine if the item is imported or exempt from tax
                            bool isImported = itemName.Contains("imported");
                            bool isExempt = itemName.Contains("book") || itemName.Contains("chocolate") || itemName.Contains("paracetamol");

                            // Create Item object and add it to the list
                            items.Add(new Item(itemName, price * quantity, isImported, isExempt, quantity));
                        }
                        else
                        {
                            // Handle parsing error
                            Console.WriteLine($"Error parsing quantity or price in input item: {inputItem}");
                        }
                    }
                    else
                    {
                        // Handle missing " at " separator
                        Console.WriteLine($"Missing ' at ' separator in input item: {inputItem}");
                    }
                }
            }

            return items;
        }
    }
}
