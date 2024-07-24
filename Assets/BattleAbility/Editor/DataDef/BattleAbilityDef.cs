using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace BattleAbility.Editor
{
    [CreateAssetMenu(fileName = "BattleAbilityData",menuName = "战斗编辑器/BattleAbilityData")] 
    public class BattleAbilityData : SerializedScriptableObject
    {
        public BattleAbilityBaseConfig baseConfig;
        public BattleAbilitySerializableTree treeData;
    }
    
    [Serializable]
    public class BattleAbilitySerializableTree
    {
        public int rootId;
        [OdinSerialize]
        public Dictionary<int,EditorBattleAbilityTreeNode> allNodes  = new();
        [OdinSerialize]
        public Dictionary<int, EditorBattleAbilityLogicEditorData> allLogicNodes = new();
        [OdinSerialize]
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