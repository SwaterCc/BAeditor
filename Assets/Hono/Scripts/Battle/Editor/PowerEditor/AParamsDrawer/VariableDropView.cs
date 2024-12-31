using System;
using System.Collections.Generic;
using System.Reflection;
using Editor.AbilityEditor.TreeItem;
using Editor.BattleEditor.AbilityEditor;
using UnityEditor.IMGUI.Controls;

namespace Editor.AbilityEditor
{
    public class VariableDropViewItem : AdvancedDropdownItem
    {
        public string VarKey;
        public string VarTypeStr;
        public Type VarType;
        
        public VariableDropViewItem(string key, Type varType, string name) : base(name)
        {
            VarKey = key;
            VarType = varType;
            VarTypeStr = VarType?.ToString().Split(".")[^1];
        }
    }

    public class VariableDropView : AdvancedDropdown
    {
        private Type _filter;
        private bool _onlyCustom;
        private ATreeItem _treeItem;
        private Action<string> _onItemSelect; 
        public VariableDropView(ATreeItem treeItem, Action<string> onItemSelect, Type filter = null, bool onlyCustom = false) : base(
            new AdvancedDropdownState())
        {
            _treeItem = treeItem;
            _onlyCustom = onlyCustom;
            _filter = filter;
            _onItemSelect = onItemSelect;
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("变量列表");

            if (_filter == null)
            {
                var dict = AbilityView.VariableBoard.KeySearch;
                foreach (var info in dict)
                {
                    root.AddChild(new VariableDropViewItem(info.Key, info.Value, info.Key + $"({info.Value})"));
                }
            }
            else
            {
                var list = AbilityView.VariableBoard.GetVariables(_filter);
                foreach (var varKey in list)
                {
                    root.AddChild(new VariableDropViewItem(varKey, _filter, varKey + $"({_filter})"));
                }
            }

            if (_onlyCustom)
            {
                return root;
            }

            if (_treeItem.TryGetFirstParent<ListenerTreeItem>(out var parentItem))
            {
                if (parentItem.Data.IsEvent)
                {
                    if (AbilityFuncInfoCache.EventCheckerDict.TryGetValue(parentItem.Data.EventType,
                                                                          out var eventEditorInfo))
                    {
                        var fields =
                            eventEditorInfo.EventInfoType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                        foreach (var fieldInfo in fields)
                        {
                            if (_filter == null)
                            {
                                root.AddChild(new VariableDropViewItem(fieldInfo.Name, fieldInfo.FieldType,
                                                                       "Event:" + fieldInfo.Name +
                                                                       $"({fieldInfo.FieldType})"));
                            }
                            else if (fieldInfo.FieldType == _filter)
                            {
                                root.AddChild(new VariableDropViewItem(fieldInfo.Name, _filter,
                                                                       "Event:" + fieldInfo.Name +
                                                                       $"({fieldInfo.FieldType})"));
                            }
                        }
                    }
                }
                else
                {
                    root.AddChild(new VariableDropViewItem("P1", typeof(object), "Msg:P1"));
                    root.AddChild(new VariableDropViewItem("P2", typeof(object), "Msg:P2"));
                    root.AddChild(new VariableDropViewItem("P3", typeof(object), "Msg:P3"));
                    root.AddChild(new VariableDropViewItem("P4", typeof(object), "Msg:P4"));
                    root.AddChild(new VariableDropViewItem("P5", typeof(object), "Msg:P5"));
                }
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            _onItemSelect.Invoke(((VariableDropViewItem)item).VarKey);
        }
    }
}