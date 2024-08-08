using System;
using System.Collections.Generic;
using Battle.Def;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleAbility.Editor
{
    [Serializable]
    public class BattleAbilityConfigItem
    {
        /// <summary>
        /// 配置ID 策划手输入
        /// </summary>
        public int configId;

        /// <summary>
        /// 能力名字
        /// </summary>
        public int name;
        
        /// <summary>
        /// 配置描述
        /// </summary>
        public string desc;
    }
    
    [CreateAssetMenu(menuName = "战斗编辑器/BattleAbilityConfigItemList")] 
    public class  BattleAbilityConfigItemList : SerializedScriptableObject
    {
        public EAbilityType abilityType = EAbilityType.UnInit;
        public Dictionary<int, BattleAbilityConfigItem> Items = new();
    }
}