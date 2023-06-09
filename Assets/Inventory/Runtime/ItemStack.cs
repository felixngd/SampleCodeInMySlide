namespace SampleCode.Inventory.Runtime
{
    public class ItemStack : IItem
    {
        public IItemData ItemData {get;}
        public uint Id {get;}
        public int Amount { get; private set; }

        public ItemStack(IItemData itemData, int amount = 1)
        {
            ItemData = itemData;
            Amount = amount;
            Id = RandomID.Generate();
        }

        public virtual bool Merge(ref ItemStack other)
        {
            if (Id == other.Id)
            {
                Amount += other.Amount;
                return true;
            }

            return false;
        }
    }

}