using System.Collections.Generic;
using NUnit.Framework;
using Script.Battle;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Editor
{
    
    public class EditorBattleAbilityTree : SerializedScriptableObject
    {
        [OdinSerialize]
        public int RootId;
        public Dictionary<int,EditorBattleAbilityTreeNode> AllNodes  = new();
        public Dictionary<int, EditorBattleAbilityLogicEditorData> AllLogicNodes = new();
    }
    
    public class EditorBattleAbilityTreeNode
    {
        public int ParentId;
        public List<int> ChildIds = new();
        public List<int> LogicNodeIds = new();
    }

    public class EditorBattleAbilityLogicEditorData
    {
        public BattleAbilityLogicNode.ENodeType ENodeType;
    }
    
}