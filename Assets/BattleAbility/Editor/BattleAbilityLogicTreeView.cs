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
    }

    public class BattleAbilityLogicTreeViewDrawer : OdinValueDrawer<BattleAbilityLogicTreeView>
    {
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var valueSerializableTree = this.ValueEntry.SmartValue.SerializableTree;
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.InfoMessageBox("這是一顆叔叔世俗叔叔叔叔和叔叔和");
            SirenixEditorGUI.EndBox();
        }
    }
}