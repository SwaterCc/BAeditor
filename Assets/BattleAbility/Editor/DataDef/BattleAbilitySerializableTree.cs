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
        /// <summary>
        /// 树的id
        /// </summary>
        public int treeId;
        /// <summary>
        /// root节点在allNodes中的下标
        /// </summary>
        public int rootIdx;
        public Dictionary<int, TreeNode> allNodes = new();
        public Dictionary<int, EditorBattleTempValue> allLocalValue = new();
        
        public class TreeNode
        {
            public int parentId;
            public List<int> childIds = new();
            public ENodeType eNodeType;
            public object nodeData;//抽象一个数据类，后期用反射实现多态
        }
    }
    
    public class EditorBattleTempValue
    {
        public int id;
        public string name;
        public string valueType;
        public string value;
        public ELocalValueRange enableRange;
    }
}