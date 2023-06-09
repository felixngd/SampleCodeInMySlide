using SampleCode.Inventory.Runtime;
using UnityEditor;
using UnityEngine;

namespace SampleCode.Inventory.Editor
{
    public class NewItemPreviewWindow : EditorWindow
    {
        public ItemDefinition Item;
        public string ItemName;

        private void OnEnable()
        {
            // Set the minimum and maximum window size
            minSize = new Vector2(300, 200);
            maxSize = new Vector2(600, 400);
        }

        private void OnGUI()
        {
            // Display the item preview
            EditorGUILayout.ObjectField("Item Preview", Item, typeof(ItemDefinition), false);

            // Display the text field for the item name
            ItemName = EditorGUILayout.TextField("Item Name", ItemName);

            // Create a button to save the item with the given name
            if (GUILayout.Button("Save Item"))
            {
                SaveItem();
            }
        }

        private void SaveItem()
        {
            Item.GenerateId();
            AssetDatabase.CreateAsset(Item, $"Assets/Inventory/Items/{ItemName}.asset");
            AssetDatabase.SaveAssets();

            // Close the window
            Close();
            
            EditorWindow.GetWindow<ItemEditor>().Reload();
        }
    }
}