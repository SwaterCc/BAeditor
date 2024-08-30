using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Hono.Scripts.Battle
{
	[ShowOdinSerializedPropertiesInInspector]
	[CreateAssetMenu(menuName = "战斗编辑器/DamageInfo")] 
    public class DamageInfo : SerializedScriptableObject
    {
        public int SourceId;
	    [ValueDropdown("GetIntEnumValues")]
	    public int SourceType;
        public int DamageConfigId;
        public int HitCount;


        private static IEnumerable<ValueDropdownItem<int>> GetIntEnumValues()
        {
	        yield return new ValueDropdownItem<int>("Skill 0", 0);
	        yield return new ValueDropdownItem<int>("Buff 1", 1);
	        yield return new ValueDropdownItem<int>("Bullet 2", 2);
        }
    }
}