using System;
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class VarCollector
    {
        private AbilityData _abilityData;
        public Dictionary<Type, List<string>> _variables = new();

        public void SetAbilityData(AbilityData abilityData)
        {
            _abilityData = abilityData;
        }

        public void RefreshAllVariable()
        {
            _variables.Clear();
            foreach (var pair in _abilityData.NodeDict)
            {
                var nodeData = pair.Value;
                if(nodeData == null)
                    continue;
                
                if (nodeData.NodeType != EAbilityNodeType.EVariableSetter)
                    continue;

                var variableNodeData = (VarSetterNodeData)nodeData;
                if (string.IsNullOrEmpty(variableNodeData.Name))
                    continue;
                var type = AbilityFunctionHelper.GetVariableType(variableNodeData.typeString);
                if(type == null) 
                    continue;
                if (!_variables.TryGetValue(type, out var list))
                {
                    list = new List<string>();
                    _variables.Add(type,list);
                }

                if (list.Contains(variableNodeData.Name))
                {
                    Debug.LogError($"配置AbilityId {_abilityData.ConfigId} 中存在重复的变量命名 name {variableNodeData.Name} ！！！");
                    
                }
                
                list.Add(variableNodeData.Name);
            }
        }

        public List<string> GetVariables(Type type)
        {
            var result = new List<string>();
            if (_variables.TryGetValue(type, out var list))
            {
                result.AddRange(list);
            }

            return result;
        }

        public void Clear()
        {
            _variables.Clear();
        }
    }
}