using System;
using System.Collections.Generic;
using System.Reflection;
using Editor.AbilityEditor.TreeItem;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using UnityEditor.IMGUI.Controls;

namespace Editor.AbilityEditor
{
    public class VariableDropViewItem : AdvancedDropdownItem
    {
        public string VarKey;
        public Type VarType;

        public VariableDropViewItem(string key, Type varType, string name) : base(name)
        {
            VarKey = key;
            VarType = varType;
        }
    }

    public class VariableDropView : AdvancedDropdown
    {
        private Type _filter;
        private bool _onlyCustom;
        private ATreeItem _treeItem;
        private Action<string, Type> _onItemSelect;

        public VariableDropView(ATreeItem treeItem,
            Action<string, Type> onItemSelect,
            Type filter = null,
            bool onlyCustom = false) : base(
            new AdvancedDropdownState())
        {
            _treeItem = treeItem;
            _onlyCustom = onlyCustom;
            _filter = filter;
            _onItemSelect = onItemSelect;
        }

        private bool filterCheck(Type type)
        {
            if (_filter == null)
                return true;
            return _filter == type;
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("变量列表");

            var list = AEditorVariableBoard.GetVariables(_treeItem.Node.Root.Cycle);
            foreach (var pVar in list)
            {
                if (filterCheck(pVar.Value))
                {
                    root.AddChild(new VariableDropViewItem(pVar.Key, pVar.Value, pVar.Key + $"({pVar.Value.Name})"));
                }
            }

            if (_onlyCustom)
            {
                return root;
            }

            //收集ListenerNode的固定变量
            addListenerTreeNodeVariable(root);

            //收集父级节点Repeat的参数
            addRepeatTreeNodeVariable(root);
            return root;
        }

        private void addListenerTreeNodeVariable(AdvancedDropdownItem root)
        {
            if (_treeItem.TryGetFirstParent<ListenerTreeItem>(out var parentItem))
            {
                if (parentItem.Data.isEvent)
                {
                    if (AbilityFuncInfoCache.EventBindInfoLookup.TryGetValue(parentItem.Data.eventType,
                                                                             out var eventEditorInfo))
                    {
                        if (eventEditorInfo.EventInfoType == null)
                        {
                            return;
                        }
                        var fields =
                            eventEditorInfo.EventInfoType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                        
                        
                        foreach (var fieldInfo in fields)
                        {
                            var fieldType = fieldInfo.FieldType.GetGenericArguments()[0]; 
                            if (filterCheck(fieldType))
                            {
                                root.AddChild(new VariableDropViewItem(fieldInfo.Name, fieldType,
                                                                       "Event:" + fieldInfo.Name +
                                                                       $"({fieldType})"));
                            }
                        }
                    }
                }
            }
        }

        private void addRepeatTreeNodeVariable(AdvancedDropdownItem root)
        {
            if (_treeItem.TryGetFirstParent<RepeatTreeItem>(out var parentItem))
            {
                if (parentItem.Data.operationType == ERepeatNodeOperationType.Repeat)
                { 
                    if (filterCheck(typeof(int)))
                    {
                        root.AddChild(new VariableDropViewItem("__LoopCount__", typeof(int), "__LoopCount__"));
                        root.AddChild(new VariableDropViewItem("__LoopValue__", typeof(int), "__LoopValue__"));
                    }
                }
                else
                {
                    var listType =Type.GetType( parentItem.Data.traverseList.paramCastType);
                    if (listType == null)
                        return;
                    var valueType = listType.GetGenericArguments()[0];
                    if (filterCheck(typeof(int)))
                    {
                        root.AddChild(new VariableDropViewItem("__LoopCount__", typeof(int), "__LoopCount__"));
                    }
                    if (filterCheck(valueType))
                    {
                        root.AddChild(new VariableDropViewItem("__LoopValue__", typeof(int), "__LoopValue__"));
                    }
                }
            }
        }
        
        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (item is VariableDropViewItem variableDropViewItem)
            {
                _onItemSelect.Invoke(variableDropViewItem.VarKey, variableDropViewItem.VarType);
            }
        }
    }
}