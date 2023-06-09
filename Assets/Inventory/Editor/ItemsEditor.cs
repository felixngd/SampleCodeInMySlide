using System;
using System.Collections.Generic;
using System.Linq;
using SampleCode.Inventory.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

namespace SampleCode.Inventory.Editor
{
    public class ItemEditor : OdinEditorWindow
    {
        [MenuItem("Tools/Item Editor")]
        private static void OpenWindow()
        {
            GetWindow<ItemEditor>().Show();
        }

        [TableList] public ItemDefinition[] items;

        [ValueDropdown("GetItemTypes")] public Type selectedType;

        protected override void OnEnable()
        {
            base.OnEnable();
            Reload();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            // Save any changes made to the ItemDefinitions here
        }

        private IEnumerable<Type> GetItemTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(ItemDefinition)) || type == typeof(ItemDefinition));
        }

        [Button(ButtonSizes.Large)]
        private void CreateNewItem()
        {
            if (selectedType == null)
            {
                Debug.LogWarning("No type selected. Please select a type to create.");
                return;
            }

            // Create a new instance of the selected item type
            ItemDefinition newItem = (ItemDefinition) ScriptableObject.CreateInstance(selectedType);
            
            // Open the NewItemPreviewWindow and pass the new item to it
            NewItemPreviewWindow previewWindow = EditorWindow.GetWindow<NewItemPreviewWindow>();
            previewWindow.Item = newItem;
            previewWindow.Show();
        }

        public void Reload()
        {
            //load all items
            items = AssetDatabase.FindAssets("t:ItemDefinition")
                .Select(guid => AssetDatabase.LoadAssetAtPath<ItemDefinition>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
        }
    }
}