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
    public abstract class ANodeSettingWindow : OdinEditorWindow
    {
        protected ATreeItem TreeItem { get; private set; }
        protected AbilityNodeData TempData { get; private set; }
        
        /// <summary>
        /// 失效关闭按钮
        /// </summary>
        protected bool DisableCloseButton { get; set; }

        public static void Open<T>(ATreeItem treeItem) where T : ANodeSettingWindow
        {
            var window = GetWindow<T>();
            window.TreeItem = treeItem;
            window.OnBeginGUI += treeItem.OnItemEditWindowOpen;
            window.OnClose += treeItem.OnItemEditWindowClose;
            window.CopyData();
            window.Init();
        }

        private void CopyData()
        {
            TempData = TreeItem.Node.DeepCopy();
        }
        
        protected abstract void Init();

        protected override void OnImGUI()
        {
            SirenixEditorGUI.BeginBox();
            EditorGUILayout.BeginVertical();
            //Desc
            Draw();
            SirenixEditorGUI.BeginBox("调试信息");
            TempData.desc = SirenixEditorFields.TextField("输入描述：", TempData.desc);
            TempData.showLog = EditorGUILayout.Toggle("是否打印运行日志：",    TempData.showLog);
            TempData.skipExecute = EditorGUILayout.Toggle("是否跳过该节点：", TempData.skipExecute);
            SirenixEditorGUI.EndBox();
            EditorGUILayout.Space(6);
            
            //应用按钮
            if (SirenixEditorGUI.Button("保存修改", ButtonSizes.Medium))
            {
                if(DisableCloseButton)
                    return;
                SaveDataToEditorNode();
                Close();
            }
            EditorGUILayout.EndVertical();
            SirenixEditorGUI.EndBox();
        }

        protected abstract void Draw();

        protected virtual void SaveDataToEditorNode()
        {
            TreeItem.Node.SaveNodeDataChange(TempData);
            TreeItem.Tree.Reload();
        }
    }

    public abstract class ANodeSettingWindow<TAbilityNodeData> : ANodeSettingWindow where TAbilityNodeData : AbilityNodeData
    {
        protected new TAbilityNodeData TempData => (TAbilityNodeData)base.TempData;

        protected sealed override void OnImGUI()
        {
            base.OnImGUI();
        }
    }
}