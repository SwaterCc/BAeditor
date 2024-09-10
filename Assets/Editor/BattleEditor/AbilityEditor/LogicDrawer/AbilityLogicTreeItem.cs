﻿using System;
using System.Collections.Generic;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public abstract class AbilityLogicTreeItem : TreeViewItem
    {
        public AbilityNodeData NodeData;

        public bool ShowFlag = true;

        public EditorWindow SettingWindow;
        
        protected AbilityLogicTreeItem(int id, int depth, string name) : base(id, depth, name) { }

        protected AbilityLogicTreeItem(AbilityNodeData nodeData) : base(nodeData.NodeId, nodeData.Depth)
        {
            NodeData = nodeData;
        }

        protected abstract Color getButtonColor();

        protected abstract string getButtonText();

        protected abstract string getItemEffectInfo();

        protected virtual float getButtonWidth()
        {
            return 256f;
        }

        protected abstract void OnBtnClicked();
        
        public virtual void DrawItem(Rect lineRect)
        {
            var bgColor = GUI.backgroundColor;
            GUI.backgroundColor = getButtonColor();
            lineRect.width = getButtonWidth();
            if (GUI.Button(lineRect, new GUIContent(getButtonText(),getItemEffectInfo())))
            {
                if (Event.current.button == 0)
                {
                    OnBtnClicked();
                }
            }
            GUI.backgroundColor = bgColor;
        }
        
        public void UpdateDepth(AbilityData data)
        {
            var parentItem = data.NodeDict[NodeData.Parent];
            NodeData.Depth = parentItem.Depth + 1;
            //this.depth = TreeNode.depth;
            if (hasChildren)
            {
                foreach (var treeViewItem in children)
                {
                    if (treeViewItem is AbilityLogicTreeItem logicTreeViewItem)
                    {
                        logicTreeViewItem.UpdateDepth(data);
                    }
                }
            }
        }
    }
    
    public interface IWindowInit
    {
        public void Init(AbilityNodeData nodeData);
        public Rect GetPos();
        public GUIContent GetWindowName();
    }

    public abstract class BaseNodeWindow<T> : EditorWindow where T : EditorWindow, IWindowInit
    {
        public static void Open(AbilityNodeData nodeData)
        {
            var window = GetWindow<T>();
            window.position = window.GetPos();
            window.titleContent = window.GetWindowName();
            window.Init(nodeData);
        }

        public AbilityNodeData NodeData;
        public Stack<EditorWindow> WindowStack = new Stack<EditorWindow>();
        public void Init(AbilityNodeData nodeData)
        {
            NodeData = nodeData;
            onInit();
        }

        protected abstract void onInit();
        
        public virtual Rect GetPos()
        {
            return GUIHelper.GetEditorWindowRect().AlignCenter(400, 600);
        }
        
        public virtual GUIContent GetWindowName()
        {
            return this.titleContent;
        }

        private void OnDestroy()
        {
            foreach (var window in WindowStack)
            {
                window.Close();
            }
            WindowStack.Clear();
        }
    }
    
    public abstract class BaseNodeOdinWindow<T> : OdinEditorWindow where T : OdinEditorWindow, IWindowInit
    {
        public static void Open(AbilityNodeData nodeData)
        {
            var window = GetWindow<T>();
            window.position = window.GetPos();
            window.titleContent = window.GetWindowName();
            window.Init(nodeData);
        }
        
        [HideInInspector]
        public AbilityNodeData NodeData;

        public void Init(AbilityNodeData nodeData)
        {
            NodeData = nodeData;
            onInit();
        }

        protected abstract void onInit();
        
        public virtual Rect GetPos()
        {
            return GUIHelper.GetEditorWindowRect().AlignCenter(400, 600);
        }
        
        public virtual GUIContent GetWindowName()
        {
            return this.titleContent;
        }
    }
    
}