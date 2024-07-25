using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleAbility.Editor
{
    [CreateAssetMenu(fileName = "BattleAbilityData", menuName = "战斗编辑器/BattleAbilityData")]
    public class BattleAbilityData : SerializedScriptableObject
    {
        public BattleAbilityBaseConfig baseConfig;
        public BattleAbilitySerializableTree treeData;
    }
}