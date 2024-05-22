namespace SalesTaxCalculator
{
    public class Item
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsImported { get; set; }
        public bool IsExempt { get; set; }
        public int Quantity { get; set; }

        // Constructor for the Item class
        public Item(string name, double price, bool isImported, bool isExempt, int quantity)
        {
            Name = name;
            Price = price;
            IsImported = isImported;
            IsExempt = isExempt;
            Quantity = quantity;
        }
    }
}
