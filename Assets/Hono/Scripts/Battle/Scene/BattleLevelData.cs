using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hono.Scripts.Battle.Scene
{
    public class BattleLevelData : MonoBehaviour
    {
        [LabelText("队伍信息")]
        public ActorGroupInfos GroupInfos = new();

        [LabelText("关卡类型")]
        public EBattleModeType BattleModeType;
        
        [LabelText("队伍出生点")]
        public PawnStartPoints GroupStartPoint;
        
        [LabelText("刷怪器")]
        public List<MonsterCreatorPoint> MonsterCreatorPoints;

        [LabelText("静态Actor(通常为触发器)")]
        public List<ActorRefreshPoint> StaticActor;

        [LabelText("路径")] 
        public Dictionary<string, List<Transform>> WayPoint;

        [LabelText("可建造区域")]
        public List<Transform> BuildPlace;
    }
}