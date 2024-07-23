using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace BattleAbility.Editor
{
    [CreateAssetMenu(fileName = "EditorBattleAbilityTree",menuName = "战斗编辑器/EditorBattleAbilityTree")] 
    public class EditorBattleAbilityTree : SerializedScriptableObject
    {
        public int rootId;
        public Dictionary<int,EditorBattleAbilityTreeNode> allNodes  = new();
        public Dictionary<int, EditorBattleAbilityLogicEditorData> allLogicNodes = new();
        public Dictionary<int, EditorBattleTempValue> allLocalValue = new();
    }
    
    [Serializable]
    public class EditorBattleAbilityTreeNode
    {
        public int parentId;
        public List<int> childIds = new();
        public List<int> logicNodeIds = new();
    }

    [Serializable]
    public class EditorBattleAbilityLogicEditorData
    {
        public BattleAbilityLogicNode.ENodeType eNodeType;
        public int nodeBehaveType;
        public List<int> paramList;
    }

    [Serializable]
    public struct EditorBattleTempValue
    {
        public string name;
        public string valueType;
        public string value;
        public int enableRange;
    }
    
}