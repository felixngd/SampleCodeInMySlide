namespace SampleCode.Inventory.Runtime
{
    public class Item : IItem
    {
        public uint Id { get; }
        public IItemData ItemData { get; }

        public Item(uint id, IItemData itemData)
        {
            Id = id;
            ItemData = itemData;
        }
    }
}