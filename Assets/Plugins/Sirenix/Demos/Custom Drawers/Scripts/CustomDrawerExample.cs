using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

[Serializable]
public class FoldoutItem
{
    public string itemName;
    public bool expanded;
}

public class CustomDrawerExample : MonoBehaviour
{
    [FoldoutGroup("Items"), ListDrawerSettings(Expanded = true)]
    public FoldoutItem[] foldoutItems;
}


public class FoldoutItemListDrawer : OdinValueDrawer<FoldoutItem[]>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        var list = this.ValueEntry.SmartValue;

        if (list == null)
        {
            list = new FoldoutItem[0];
        }

        for (int i = 0; i < list.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            
            bool isExpanded = SirenixEditorGUI.Foldout(list[i].expanded,list[i].itemName);

            if (GUILayout.Button("↑", GUILayout.Width(30)) && i > 0)
            {
                // Swap with the previous item
                (list[i - 1], list[i]) = (list[i], list[i - 1]);
            }

            if (GUILayout.Button("↓", GUILayout.Width(30)) && i < list.Length - 1)
            {
                // Swap with the next item
                (list[i + 1], list[i]) = (list[i], list[i + 1]);
            }

            EditorGUILayout.EndHorizontal();

            if (isExpanded)
            {
                EditorGUI.indentLevel++;
                SirenixEditorFields.TextField("Item Name");
                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Add Item"))
        {
            Array.Resize(ref list, list.Length + 1);
            list[list.Length - 1] = new FoldoutItem { itemName = "New Item " + list.Length };
        }

        this.ValueEntry.SmartValue = list;
    }
}
