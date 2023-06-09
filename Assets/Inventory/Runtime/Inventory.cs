using System.Collections.Generic;


namespace SampleCode.Inventory.Runtime
{
    public class Inventory : IInventory
    {
        private List<IItem> _items;
            // Property to expose the list of items
        public IReadOnlyList<IItem> Items => _items.AsReadOnly();


        public Inventory()
        {
            _items = new List<IItem>();
        }

        public void AddItem(IItem item)
        {
            _items.Add(item);
        }

        public void RemoveItem(IItem item)
        {
            _items.Remove(item);
        }

        public int GetTotalItems()
        {
            return _items.Count;
        }
    }
}