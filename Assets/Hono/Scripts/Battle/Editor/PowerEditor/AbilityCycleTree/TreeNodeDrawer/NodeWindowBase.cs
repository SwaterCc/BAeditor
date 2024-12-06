using System;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public abstract class AbilityEditorWindow : OdinEditorWindow
    {
        protected ATreeItem TreeItem { get; private set; }
        protected AbilityNodeData TempData { get; private set; }
        public static void Open<T>(ATreeItem treeItem) where T : AbilityEditorWindow
        {
            var window = GetWindow<T>();
            window.TreeItem = treeItem;
            window.OnBeginGUI += treeItem.OnItemEditWindowOpen;
            window.OnClose += treeItem.OnItemEditWindowClose;
            window.CopyData();
            window.Init();
        }

        protected virtual void CopyData()
        {
            TempData = TreeItem.EditorNode.DeepCopy();
        }
        
        protected abstract void Init();

        protected override void OnImGUI()
        {
            base.OnImGUI();
            SirenixEditorGUI.BeginBox();
            EditorGUILayout.BeginVertical();
            //Desc
            TempData.Desc = SirenixEditorFields.TextField("输入描述：", TempData.Desc);
            Draw();
            EditorGUILayout.Space(6);
            
            //应用按钮
            if (SirenixEditorGUI.Button("保存修改", ButtonSizes.Medium))
            {
                SaveDataToEditorNode();
                Close();
            }
            EditorGUILayout.EndVertical();
        }

        protected abstract void Draw();

        protected virtual void SaveDataToEditorNode()
        {
            TreeItem.EditorNode.SaveNodeDataChange(TempData);
        }
    }

    public abstract class AbilityEditorWindow<TAbilityNodeData> : AbilityEditorWindow where TAbilityNodeData : AbilityNodeData
    {
        protected new TAbilityNodeData TempData { get; private set; }

        protected sealed override void CopyData()
        {
            TempData = TreeItem.EditorNode.DeepCopy<TAbilityNodeData>();
        }

        protected sealed override void OnImGUI()
        {
            base.OnImGUI();
        }
    }
}