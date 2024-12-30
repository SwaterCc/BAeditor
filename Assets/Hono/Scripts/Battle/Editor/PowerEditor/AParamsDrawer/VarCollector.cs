using System;
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class AbilityEditorVariableBoard
    {
        private AbilityData _abilityData;
        public Dictionary<Type, List<string>> _variables = new();

        public void SetAbilityData(AbilityData abilityData)
        {
            _abilityData = abilityData;
            RefreshAllVariable();
        }

        public void RefreshAllVariable()
        {
            _variables.Clear();
            foreach (var pair in _abilityData.NodeDict)
            {
                var nodeData = pair.Value;
                if(nodeData == null)
                    continue;
                
                if (nodeData is not VariableNodeData variableNodeData)
                    continue;

                if (string.IsNullOrEmpty(variableNodeData.key))
                    continue;

                var type = Type.GetType(variableNodeData.valueType);
                if(type == null) 
                    continue;
                
                if (!_variables.TryGetValue(type, out var list))
                {
                    list = new List<string>();
                    _variables.Add(type,list);
                }

                if (list.Contains(variableNodeData.key))
                {
                    
                    
                }
                
                list.Add(variableNodeData.key);
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