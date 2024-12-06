
using System;
using Hono.Scripts.Battle;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.TreeItem
{
    public class RepeatTreeItem : ATreeItem<RepeatNodeData>
    {
        private new RepeatNodeData _nodeData;

        public RepeatTreeItem(AbilityCycleTree tree, AbilityNodeData data) : base(tree, data)
        {
            _nodeData = (RepeatNodeData)base._nodeData;
        }

        protected override void buildMenu()
        {
            _menu.AddItem(new GUIContent("创建节点/添加Action"), false,
                AddChild, (EAbilityNodeType.EAction));
            _menu.AddItem(new GUIContent("创建节点/添加If"), false,
                AddChild, (EAbilityNodeType.EBranchControl));
            _menu.AddItem(new GUIContent("创建节点/Set变量"), false,
                AddChild, (EAbilityNodeType.EVariableSetter));
            _menu.AddItem(new GUIContent("创建节点/SetAttr"), false,
                AddChild, (EAbilityNodeType.EAttrSetter));
            
            if (checkHasParent(EAbilityNodeType.ETimer))
            {
                _menu.AddItem(new GUIContent("创建节点/创建Timer节点"), false,
                    AddChild, (EAbilityNodeType.ETimer));
            }
            _menu.AddItem(new GUIContent("删除"), false,
                Remove);
        }
        
        protected override AbilityNodeWindow getSettingWindow()
        {
            throw new NotImplementedException();
        }

        protected override void OnCopy()
        {
            throw new NotImplementedException();
        }

        protected override void OnMove()
        {
            throw new NotImplementedException();
        }

        protected override void OnAdd()
        {
            throw new NotImplementedException();
        }

        protected override void OnRemove()
        {
            throw new NotImplementedException();
        }

        protected override void buildMenu(GenericMenu menu)
        {
            throw new NotImplementedException();
        }

        protected override void OnBtnClicked(Rect btnRect)
        {
            AbilityViewDrawer.NodeBtnClick(_nodeData);
            SettingWindow = NodeWindowBase<RepeatNodeDataWindow, RepeatNodeData>.GetSettingWindow(Tree.TreeData,
                _nodeData,
                (nodeData) => { Tree.TreeData.NodeDict[nodeData.NodeId] = nodeData;
                    _nodeData = nodeData;
                });
            SettingWindow.position = new Rect(btnRect.x, btnRect.y, 740, 140);
            SettingWindow.Show();
        }
    }

    public class RepeatNodeDataWindow : NodeWindowBase<RepeatNodeDataWindow,RepeatNodeData>, IAbilityNodeWindow<RepeatNodeData>
    {

        private ParameterField _maxCount;
        protected override void onInit()
        {
            _maxCount = new ParameterField(_nodeData.MaxRepeatCount, "循环次数", typeof(int));
        }

        private void OnGUI()
        {
            SirenixEditorGUI.BeginBox("设置循环次数");
            
            _maxCount.Draw();
            
            if (SirenixEditorGUI.Button("保  存", ButtonSizes.Medium))
            {
                Save();
            }
            SirenixEditorGUI.EndBox();
        }
    }
}