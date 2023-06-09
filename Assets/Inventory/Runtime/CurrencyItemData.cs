namespace SampleCode.Inventory.Runtime
{
    public class CurrencyItemData : IItemData
    {
        public uint DefinitionId { get; }
        public int Value { get; private set; }

        public CurrencyItemData(uint definitionId, int value)
        {
            DefinitionId = definitionId;
            Value = value;
        }

        public IItemData Clone()
        {
            return new CurrencyItemData(DefinitionId, Value);
        }

        public bool CanCombine(in IItemData other)
        {
            return other is CurrencyItemData && DefinitionId == other.DefinitionId;
        }

        public bool Combine(ref IItemData other)
        {
            if (CanCombine(other))
            {
                Value += ((CurrencyItemData)other).Value;
                return true;
            }

            return false;
        }

        public static CurrencyItemData operator +(CurrencyItemData a, CurrencyItemData b)
        {
            return new CurrencyItemData(a.DefinitionId, a.Value + b.Value);
        }

        public static CurrencyItemData operator -(CurrencyItemData a, CurrencyItemData b)
        {
            return new CurrencyItemData(a.DefinitionId, a.Value - b.Value);
        }
    
    }

}