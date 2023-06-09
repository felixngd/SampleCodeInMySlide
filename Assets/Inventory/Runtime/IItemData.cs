namespace SampleCode.Inventory.Runtime
{
    public interface IItemData
    {
        uint DefinitionId { get; }

        IItemData Clone();

        bool CanCombine(in IItemData other);

        bool Combine(ref IItemData other);
    }
}