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
        public List<int> ReadyStageAbilityIds = new();
        
        [LabelText("回合运行期执行的Ability")]
        public List<int> RunningAbilityIds = new();
     
        [BoxGroup("结算条件")]
        [LabelText("检测时长(-1为无限时长，但是所有成功条件会强制变成即刻结算)")]
        public float RunningCheckTime;
  
        [BoxGroup("结算条件")]
        [InfoBox("非立即结算的成功条件必须全部满足，最终结算时才会判定当前波次通过")]
        [LabelText("成功条件")]
        public List<RoundScoreCondition> SuccessConditions = new();
   
        [BoxGroup("结算条件")]
        [LabelText("失败结算条件(全为即刻结算)")]
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
    public class RoundScoreCondition
    {
        [LabelText("结算条件类型1")]
        public ERoundConditionType ConditionType;
        [LabelText("结算条件类型1参数")]
        public int ConditionParam;
        [LabelText("结算条件类型2")]
        public ERoundTargetType TargetType;
        [LabelText("结算条件类型2参数")]
        public int TargetParam;
        [LabelText("是否中止回合立刻结算")]
        public bool ScoreNow;
    }
    
    [Serializable]
    public class FailedCondition : RoundScoreCondition
    {
        public FailedCondition()
        {
            ScoreNow = true;
        }
    }
    
    [Serializable]
    public struct MonsterBuilderLinkInfo
    {
        public int MonsterBuilderUid;
        public int ConfigId;
    }
}