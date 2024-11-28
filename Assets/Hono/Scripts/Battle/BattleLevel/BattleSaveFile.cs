#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public class BattleSaveFile
    {
        private static RangeFilterSetting _buildingRangeFilter = new()
        {
            OpenBoxCheck = false,
            /*Ranges = new List<FilterCondition>()
            {
                new() { conditionType = EFilterConditionType.ActorType, value = (int)EActorType.Building, }
            }*/
        };


        public int SceneId;

        public int RoundCount;

        public int RpCount;

        public struct BuildingInfo
        {
            public int ConfigId;
            public Vector3 Pos;
        }

        public List<BuildingInfo> BuildingInfos = new();


        public void SaveData(BattleGround ground)
        {
            SceneId = ground.BattleGroundConfigId;
            RoundCount = ground.RtInfo.CurRoundCount;
            RpCount = ground.RtInfo.RPCount;
            /*foreach (var uid in ActorManager.Instance.UseFilter(BattleManager.BattleController.Self,
                         _buildingFilter)) {
                var building = ActorManager.Instance.GetActor(uid);
                BuildingInfos.Add(new BuildingInfo() { ConfigId = building.ConfigId, Pos = building.Pos });
            }*/
        }
    }
}