using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle.Scene
{
    public class BattleLevelData : ScriptableObject
    {
        [LabelText("队伍信息")]
        public PawnGroupInfos GroupInfos = new();

        [LabelText("关卡类型")]
        public EBattleModeType BattleModeType;
        
        [LabelText("队伍出生点")]
        public PawnStartPoints GroupStartPoint;
        
        [LabelText("刷怪器")]
        public List<MonsterCreatorPoint> MonsterCreatorPoints;

        [LabelText("静态Actor数据结构")]
        public List<ActorRefreshPoint> StaticActors;

        [LabelText("预设路径")] 
        public Dictionary<string, List<Vector3>> WayPoint;
    }
}