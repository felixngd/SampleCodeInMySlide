namespace SampleCode.Inventory.Runtime
{
    public interface IInventory
    {
        void AddItem(IItem item);
        void RemoveItem(IItem item);
        int GetTotalItems();
    }
}