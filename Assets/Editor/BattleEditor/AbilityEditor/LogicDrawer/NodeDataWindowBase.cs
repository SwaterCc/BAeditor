using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public interface IWindowInit
    {
        public void Init(AbilityNodeData nodeData);
        public Rect GetPos();
        public GUIContent GetWindowName();
    }


    public abstract class BaseNodeWindow<T> : EditorWindow where T : EditorWindow, IWindowInit
    {
        public static EditorWindow GetWindow(AbilityNodeData nodeData, Action onSave)
        {
            var window = GetWindow<T>();
            window.position = window.GetPos();
            window.titleContent = window.GetWindowName();
            window.Init(nodeData);
            return window;
        }

        protected Action _onSave;
        protected AbilityNodeData _nodeData;

        public void Init(AbilityNodeData nodeData)
        {
            _nodeData = nodeData;
            onInit();
        }

        protected abstract void onInit();

        public virtual Rect GetPos()
        {
            return GUIHelper.GetEditorWindowRect().AlignCenter(740, 600);
        }

        public virtual GUIContent GetWindowName()
        {
            return this.titleContent;
        }

        public void Save()
        {
            _onSave?.Invoke();
        }
    }
}