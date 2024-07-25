using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace BattleAbility.Editor
{
    /// <summary>
    /// 用系列化的数据还原树状图
    /// </summary>
    public class BattleAbilityLogicTreeView
    {
        private BattleAbilitySerializableTree _serializableTree;
        public BattleAbilitySerializableTree SerializableTree => _serializableTree; 
        public BattleAbilityLogicTreeView(BattleAbilitySerializableTree serializableTree)
        {
            _serializableTree = serializableTree;
        }

        /// <summary>
        /// 绘制树
        /// </summary>
        public void BuildTree()
        {
            
        }
    }
}