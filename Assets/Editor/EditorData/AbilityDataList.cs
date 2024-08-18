using System;
using System.Collections.Generic;
using Battle;
using Battle.Def;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleAbility.Editor
{
    [Serializable]
    public class AbilityConfigItem
    {
        /// <summary>
        /// 配置ID 策划手输入
        /// </summary>
        public int configId;

        /// <summary>
        /// 能力名字
        /// </summary>
        public string name;
        
        /// <summary>
        /// 配置描述
        /// </summary>
        public string desc;
    }
    
    [CreateAssetMenu(menuName = "战斗编辑器/AbilityDataList")] 
    public class  AbilityDataList : SerializedScriptableObject
    {
        public EAbilityType abilityType;
        public Dictionary<int, AbilityConfigItem> Items = new();
    }
}