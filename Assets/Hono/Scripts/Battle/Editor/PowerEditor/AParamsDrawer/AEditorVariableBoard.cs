using System;
using System.Collections.Generic;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public class AEditorVariableBoard
    {
        private AbilityData _abilityData;
        private readonly Dictionary<Type, List<string>> _variables = new();
        public Dictionary<string, Type> KeySearch { get; } = new();


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

                if (nodeData is VariableNodeData variableNodeData)
                {
                    if (variableNodeData.isModify)
                        continue;

                    if (string.IsNullOrEmpty(variableNodeData.key))
                        continue;

                    var type = Type.GetType(variableNodeData.valueType);
                    if (type == null)
                        continue;

                    addVariable(type, variableNodeData.key);
                }

                if (nodeData is ActionNodeData actionNodeData)
                {
                    if (!actionNodeData.isCreateVariable)
                        continue;
                    var type = Type.GetType(actionNodeData.returnType);
                    if (type == null)
                        continue;
                    if (string.IsNullOrEmpty(actionNodeData.returnValueKey))
                        continue;
                    addVariable(type, actionNodeData.returnValueKey);
                }

                if (nodeData is RepeatNodeData repeatNodeData) { }
            }
        }

        private void addVariable(Type varType, string varKey)
        {
            if (!KeySearch.TryAdd(varKey, varType))
            {
                return;
            }

            if (!_variables.TryGetValue(varType, out var list))
            {
                list = new List<string>();
                _variables.Add(varType, list);
            }

            if (!list.Contains(varKey))
            {
                list.Add(varKey);
            }
        }

        public bool HasVariable(string key)
        {
            return KeySearch.ContainsKey(key);
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
    }
}