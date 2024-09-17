using System;
using Hono.Scripts.Battle;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public interface IAbilityNodeWindow<out TNodeData>
    {
        public void Init(AbilityNodeData nodeData, Action<TNodeData> onSave);
    }

    public abstract class BaseNodeWindow<T,TNodeData> : EditorWindow where T : EditorWindow, IAbilityNodeWindow<TNodeData> where TNodeData : AbilityNodeData
    {
        public static EditorWindow GetSettingWindow(AbilityData treeData, TNodeData nodeData,
            Action<TNodeData> onSave)
        {
            var window = GetWindow<T>();
            var copy = treeData.DeepCopyNodeData(nodeData);
            window.Init(copy, onSave);
            return window;
        }

        protected TNodeData _nodeData;
        protected Action<TNodeData> _onSave;

        public void Init(AbilityNodeData nodeData, Action<TNodeData> onSave)
        {
            _nodeData = (TNodeData)nodeData;
            _onSave = onSave;
            onInit();
        }

        protected abstract void onInit();
        
        protected void Save()
        {
            _onSave.Invoke(_nodeData);
            Close();
        }
    }
}