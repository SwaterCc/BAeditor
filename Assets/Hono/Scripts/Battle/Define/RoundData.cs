using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class RoundData
    {
        [LabelText("准备期时长(-1为无限时长)")]
        public float ReadyStageTime;

        [LabelText("准备期执行的Ability")]
        public int ReadyStageAbilityId = -1;

        [LabelText("运行时长(-1为无限时长)")]
        public float RunningStageTime;

        [LabelText("是否使用Ability控制回合运行")]
        public bool UseAbilityControlRunning;
        
        [ShowIf("UseAbilityControlRunning")]
        [LabelText("AbilityConfigId")]
        public int RunningAbilityConfigId;
        
        [HideIf("UseAbilityControlRunning")]
        [LabelText("当前波次唤醒的刷怪器")]
        public List<MonsterBuilderLinkInfo> MonsterBuilderLinkInfos = new();
        
        [HideIf("UseAbilityControlRunning")]
        [LabelText("当前波次唤醒的触发器")]
        public List<int> TriggerBoxLinkInfos = new();
        
        [LabelText("成功结算条件")]
        public List<RoundCondition> SuccessConditions = new();
            
        [LabelText("失败结算条件")]
        public List<RoundCondition> FailedConditions = new();

        [LabelText("可重复挑战该波次")]
        public bool CanRepeat;
    }

    [Serializable]
    public struct RoundCondition
    {
        public ERoundConditionType ConditionType;
        public int Value;
    }

    [Serializable]
    public struct MonsterBuilderLinkInfo
    {
        public int MonsterBuilderUid;
        public int ConfigId;
    }
}