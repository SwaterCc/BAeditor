using System.Collections.Generic;
using BattleAbility.Editor.BattleAbilityCustomAttribute;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleAbility.Editor
{
    [CreateAssetMenu(fileName = "BattleAbilityData", menuName = "战斗编辑器/BattleAbilityData")]
    public class BattleAbilityData : SerializedScriptableObject
    {
        public BattleAbilityBaseConfig baseConfig;
        public List<BattleAbilityLogicStage> stageDatas = new();
    }

    public class BattleAbilityLogicStage
    {
        [BattleAbilityLabelTagEditor("阶段ID", BattleAbilityLabelTagEditor.ELabeType.Int32)]
        public int stageId;

        [BattleAbilityLabelTagEditor("阶段标志", BattleAbilityLabelTagEditor.ELabeType.Enum)]
        public EStageTag stageTag;

        [BattleAbilityLabelTagEditor("阶段动画路径")]
        public string animPath;

        [BattleAbilityLabelTagEditor("阶段音效路径")]
        public string audioPath;

        public List<BattleAbilitySerializableTree> SerializableTrees = new();
    }
}