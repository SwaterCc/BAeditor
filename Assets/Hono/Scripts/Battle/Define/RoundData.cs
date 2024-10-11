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
        
        [LabelText("是否使用Ability控制回合运行")]
        public bool UseAbilityControlRunning;
        
        [ShowIf("UseAbilityControlRunning")]
        [LabelText("AbilityConfigId")]
        public int RunningAbilityConfigId;
        
        [HideIf("UseAbilityControlRunning")]
        [BoxGroup("成功结算条件")]
        [LabelText("超过指定时间后胜利(<=0 相当于不判定时间)")]
        public float SuccessTime;
        
        [HideIf("UseAbilityControlRunning")]
        [BoxGroup("成功结算条件")]
        [LabelText("击杀指定阵营全部的单位")]
        public List<int> KillFactionIds = new();
        
        [HideIf("UseAbilityControlRunning")]
        [BoxGroup("失败结算条件")]
        [LabelText("超过指定时间后失败(<=0 相当于不判定时间)")]
        public float FailedTime;
        
        [HideIf("UseAbilityControlRunning")]
        [BoxGroup("失败结算条件")]
        [LabelText("指定阵营单位全部死亡后失败")]
        public List<int> FailedFactionIds = new();
        
        [LabelText("结算期时长(必须为有限时长最低为0)")]
        public float ScoringStageTime = 0;
        
        [LabelText("当前波次唤醒的刷怪器")]
        public List<MonsterBuilderLinkInfo> MonsterBuilderLinkInfos = new();
        
        [LabelText("当前波次唤醒的触发器")]
        public List<int> TriggerBoxLinkInfos = new();
    }
    
    [Serializable]
    public struct MonsterBuilderLinkInfo
    {
        public int MonsterBuilderUid;
        public int ConfigId;
    }
}