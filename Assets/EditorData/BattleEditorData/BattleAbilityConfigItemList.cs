using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace EditorData.BattleEditorData
{
    [Serializable]
    public class BattleAbilityConfigItem
    {
        /// <summary>
        /// 配置ID 策划手输入
        /// </summary>
        public int ConfigId;

        /// <summary>
        /// 配置描述
        /// </summary>
        public string Desc;
    }
    
    [CreateAssetMenu(menuName = "战斗编辑器/BattleAbilityConfigItemList")] 
    public class  BattleAbilityConfigItemList : SerializedScriptableObject
    {
        public Dictionary<int, BattleAbilityConfigItem> Items = new();
    }
}