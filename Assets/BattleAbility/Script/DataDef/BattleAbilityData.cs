using System.Collections.Generic;
using Battle.Def;
using BattleAbility.Editor;
using BattleAbility.Editor.BattleAbilityCustomAttribute;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleAbility
{
    [CreateAssetMenu(fileName = "BattleAbilityData", menuName = "战斗编辑器/BattleAbilityData")]
    public class BattleAbilityData : SerializedScriptableObject
    {
        public BattleAbilityBaseConfig baseConfig;
        public List<BattleAbilityLogicStage> stageDatas = new();
    }

    public class BattleAbilityLogicStage
    {
        [BAEditorShowLabelTag("阶段ID", BAEditorShowLabelTag.ELabeType.Int32)]
        public int stageId;

        [BAEditorShowLabelTag("阶段标志", BAEditorShowLabelTag.ELabeType.Enum)]
        public EStageTag stageTag;

        [BAEditorShowLabelTag("阶段动画路径")]
        public string animPath;

        [BAEditorShowLabelTag("阶段音效路径")]
        public string audioPath;
        
        public List<BattleAbilitySerializableTree> SerializableTrees = new();
    }
}