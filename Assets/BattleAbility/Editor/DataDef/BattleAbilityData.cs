using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleAbility.Editor
{
    [CreateAssetMenu(fileName = "BattleAbilityData", menuName = "战斗编辑器/BattleAbilityData")]
    public class BattleAbilityData : SerializedScriptableObject
    {
        public BattleAbilityBaseConfig baseConfig ;
        public List<BattleAbilityLogicStage> stageDatas = new();
    }
    
    public class BattleAbilityLogicStage
    {
        public int stageId;
        public EStageTag stageTag;
        public string animPath;
        public string audioPath;

        public List<BattleAbilitySerializableTree> SerializableTrees = new();
    }
}