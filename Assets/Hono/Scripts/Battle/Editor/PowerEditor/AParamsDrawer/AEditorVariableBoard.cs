using System;
using System.Collections.Generic;
using System.Linq;
using Editor.BattleEditor.AbilityEditor;
using Hono.Scripts.Battle;
using UnityEngine;

namespace Editor.AbilityEditor
{
    public static class AEditorVariableBoard
    {
        public static Dictionary<EAbilityCycle, Dictionary<string, Type>> Variables = new();

        public static bool HasVariable(string key)
        {
            foreach (var collection in Variables)
            {
                if (collection.Value.ContainsKey(key))
                {
                    return true;
                }
            }

            return false;
        }
        
        public static Dictionary<string,Type> GetVariables(EAbilityCycle cycle)
        {
            var result = new Dictionary<string,Type>();
            for (int i = 1; i <= (int)cycle; i++)
            {
                if (Variables.TryGetValue((EAbilityCycle)i, out var dict))
                {
                    foreach (var pVar in dict)
                    {
                        result.Add(pVar.Key, pVar.Value);
                    }
                }
            }

            return result;
        }

        public static void Clear()
        {
            Variables.Clear();
        }

        public static void CycleVariableRefresh(AEditorTreeHeadNode cycleHeadNode)
        {
            if (!Variables.TryGetValue(cycleHeadNode.Cycle, out var varCollection))
            {
                varCollection = new Dictionary<string, Type>();
                Variables.Add(cycleHeadNode.Cycle, varCollection);
            }

            varCollection.Clear();

            deepNodeAndAddVariable(cycleHeadNode, varCollection);
        }

        private static void addVariable(AbilityNodeData nodeData, Dictionary<string, Type> collection)
        {
            if (nodeData is VariableNodeData variableNodeData)
            {
                if (variableNodeData.isModify)
                    return;

                if (string.IsNullOrEmpty(variableNodeData.key))
                    return;

                var type = Type.GetType(variableNodeData.valueType);
                if (type == null)
                    return;

                if (!collection.TryAdd(variableNodeData.key, type))
                {
                    Debug.LogError($"变量重复添加 key:{variableNodeData.key}");
                }
            }

            if (nodeData is ActionNodeData actionNodeData)
            {
                if (!actionNodeData.isCreateVariable)
                    return;
                var type = Type.GetType(actionNodeData.returnType);
                if (type == null)
                    return;
                if (string.IsNullOrEmpty(actionNodeData.returnValueKey))
                    return;
                if (!collection.TryAdd(actionNodeData.returnValueKey, type))
                {
                    Debug.LogError($"变量重复添加 key:{actionNodeData.returnValueKey}");
                }
            }

            if (nodeData is RepeatNodeData repeatNodeData) { }
        }

        private static void deepNodeAndAddVariable(AEditorTreeNode node, Dictionary<string, Type> collection)
        {
            var nodeData = node.Data;
            addVariable(nodeData, collection);

            foreach (var nodeChild in node.Children)
            {
                deepNodeAndAddVariable(nodeChild, collection);
            }
        }
    }
}