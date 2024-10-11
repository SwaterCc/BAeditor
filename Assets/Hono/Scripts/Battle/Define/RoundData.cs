using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle
{
    [Serializable]
    public class RoundData
    {
        [LabelText("准备期时长(-1为无限时长)")]
        public float ReadyStageTime;

        [LabelText("准备期执行的Ability")]
        public int ReadyStageAbilityId = -1;
        
        [LabelText("自定义回合结束")]
        public bool Custom;
        
        [ShowIf("Custom")]
        [LabelText("使用指定Ability控制")]
        public int RunningAbilityConfigId;
        
        [HideIf("UseAbilityControlRunning")]
        [BoxGroup("结算条件")]
        [LabelText("检测时长(-1为无限时长，但是所有成功条件会强制变成即刻结算)")]
        public float RunningCheckTime;
        
        [HideIf("UseAbilityControlRunning")]
        [BoxGroup("结算条件")]
        [LabelText("成功条件(不允许出现重复条件)")]
        public List<SuccessCondition> SuccessConditions = new();
        
        [HideIf("UseAbilityControlRunning")]
        [BoxGroup("结算条件")]
        [LabelText("失败结算条件(不允许出现重复条件)(全为即刻结算)")]
        public List<FailedCondition> FailedConditions = new();
        
        [LabelText("成功结算期时长(必须为有限时长最低为0)")]
        public float SucessScoringStageTime = 0;
        
        [LabelText("失败结算期时长(必须为有限时长最低为0)")]
        public float FailedScoringStageTime = 0;
        
        [LabelText("当前波次唤醒的刷怪器")]
        public List<MonsterBuilderLinkInfo> MonsterBuilderLinkInfos = new();
        
        [LabelText("当前波次唤醒的触发器")]
        public List<int> TriggerBoxLinkInfos = new();
    }

    [Serializable]
    public class SuccessCondition
    {
        public ERoundConditionType ConditionType;
        [LabelText("是否立刻结算")]
        public bool Flag;
        public List<int> Params = new ();
    }
    
    [Serializable]
    public class FailedCondition
    {
        public ERoundConditionType ConditionType;
        public List<int> Params = new ();
    }
    
    [Serializable]
    public struct MonsterBuilderLinkInfo
    {
        public int MonsterBuilderUid;
        public int ConfigId;
    }
}