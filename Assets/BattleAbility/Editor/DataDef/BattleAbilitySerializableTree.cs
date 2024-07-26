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
        public int treeId;
        public int rootId;
        public Dictionary<int, TreeNodeIndex> allNodes = new();
        public Dictionary<int, NodeData> allNodeLogics = new();
        public Dictionary<int, EditorBattleTempValue> allLocalValue = new();
        
        public class TreeNodeIndex
        {
            public int parentId;
            public int dataIndex;
            public List<int> childIds = new();
        }
        
        public class NodeData
        {
            public BattleAbilityLogicNode.ENodeType eNodeType;
            public List<int> paramList;
        }
    }

    
    public class EditorBattleTempValue
    {
        public string name;
        public string valueType;
        public string value;
        public ELocalValueRange enableRange;
        //public 
    }
}