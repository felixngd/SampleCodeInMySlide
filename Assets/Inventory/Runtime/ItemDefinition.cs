using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace SampleCode.Inventory.Runtime
{
    [CreateAssetMenu(fileName = "ItemDefinition", menuName = "Inventory/ItemDefinition")]
    public class ItemDefinition : ScriptableObject, IItemData
    {
        [InlineButton("GenerateId")][SerializeField, ReadOnly] private uint definitionId;
        [SerializeField] private Sprite icon;
        
        public uint DefinitionId => definitionId;
        
        public IItemData Clone()
        {
            var clone = CreateInstance<ItemDefinition>();
            clone.definitionId = definitionId;
            clone.icon = icon;
            clone.definitionId = RandomID.Generate();
            
            return clone;
        }

        public bool CanCombine(in IItemData other)
        {
            return other is ItemDefinition && definitionId == other.DefinitionId;
        }

        public bool Combine(ref IItemData other)
        {
            if (CanCombine(other))
            {
                // Implement any logic for combining items here
                return true;
            }

            return false;
        }
        
        public void GenerateId()
        {
            definitionId = RandomID.Generate();
        }
    }
}