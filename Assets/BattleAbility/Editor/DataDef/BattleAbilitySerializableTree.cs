using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace BattleAbility.Editor
{
    public class BattleAbilitySerializableTree
    {
        public int rootId;
        public Dictionary<int, EditorBattleAbilityTreeNode> allNodes = new();
        public Dictionary<int, EditorBattleAbilityLogicEditorData> allLogicNodes = new();
        public Dictionary<int, EditorBattleTempValue> allLocalValue = new();
    }


    public class EditorBattleAbilityTreeNode
    {
        public int parentId;
        public List<int> childIds = new();
        public List<int> logicNodeIds = new();
    }


    public class EditorBattleAbilityLogicEditorData
    {
        public BattleAbilityLogicNode.ENodeType eNodeType;
        public int nodeBehaveType;
        public List<int> paramList;
    }


    public class EditorBattleTempValue
    {
        public string name;
        public string valueType;
        public string value;
        public int enableRange;
    }
}