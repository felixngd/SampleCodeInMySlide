namespace SampleCode.Inventory.Runtime
{
    public interface IItem
    {
        uint Id { get; }
        IItemData ItemData { get; }
    }
}