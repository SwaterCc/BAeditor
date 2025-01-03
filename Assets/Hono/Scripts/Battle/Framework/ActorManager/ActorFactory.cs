#region

using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class ActorManager
    {
        /// <summary>
        /// 游戏主要对象生成器，玩家角色，怪物，建筑
        /// </summary>
        private static class MajorActorFactory
        {
            public static bool ActorSetup(ref Actor actor, in int configId, ActorModel actorModel)
            {
                if (configId <= 0)
                {
                    return false;
                }

                actor.SetAttr(EAttrType.AttrConfigId, configId, false);

                int modelId = 0;
                ActorLogic logic = null;

                switch (actor.ActorType)
                {
                    case EActorType.Pawn:
                        if (!ConfigManager.Table<PawnLogicTable>().TryGet(configId, out var pawnLogicRow))
                        {
                            Debug.LogError($"PawnLogicTable 创建找不到对应的配置 configId {configId}");
                            return false;
                        }

                        PawnLogic pawnLogic = APool<PawnLogic>.Pool.Rent();
                        pawnLogic.PawnLogicRow = pawnLogicRow;
                        logic = pawnLogic;
                        modelId = pawnLogicRow.ModelId;
                        break;
                    case EActorType.Monster:
                        if (!ConfigManager.Table<MonsterLogicTable>().TryGet(configId, out var monsterLogicRow))
                        {
                            Debug.LogError($"PawnLogicTable 创建找不到对应的配置 configId {configId}");
                            return false;
                        }

                        MonsterLogic monsterLogic = APool<MonsterLogic>.Pool.Rent();
                        monsterLogic.MonsterConfig = monsterLogicRow;
                        logic = monsterLogic;
                        modelId = monsterLogicRow.ModelId;
                        break;
                    case EActorType.Building:
                        if (!ConfigManager.Table<BuildingLogicTable>().TryGet(configId, out var buildingLogicRow))
                        {
                            Debug.LogError($"PawnLogicTable 创建找不到对应的配置 configId {configId}");
                            return false;
                        }

                        BuildingLogic buildingLogic = APool<BuildingLogic>.Pool.Rent();
                        buildingLogic.BuildingConfig = buildingLogicRow;
                        logic = buildingLogic;
                        modelId = buildingLogicRow.Model;
                        break;
                    default:
                        Debug.LogError("非法ActorType");
                        return false;
                }

                if (actorModel == null)
                {
                    if (!ConfigManager.Table<ModelTable>().TryGet(modelId, out var modelRow))
                    {
                        Debug.LogError($"ModelTable 创建找不到对应的配置 configId {configId}");
                        //找不到应该使用原型模型
                        actor.ModelController.ModelPath = modelRow.ModelPath;
                        actor.ModelController.Radius = modelRow.ModelRadius;
                    }
                    else
                    {
                        actor.ModelController.ModelPath = modelRow.ModelPath;
                        actor.ModelController.Radius = modelRow.ModelRadius;
                    }
                }
                else
                {
                    actor.ModelController.Model = actorModel;
                }

                actor.Setup(logic);

                return true;
            }
        }


        /// <summary>
        /// 打击盒,掉落物和子弹的生成器
        /// </summary>
        private static class DynamicActorFactory
        {
            public static bool ActorSetup(ref Actor actor)
            {
                ActorLogic logic = null;
                switch (actor.ActorType)
                {
                    case EActorType.Bullet:
                        logic = APool<BulletLogic>.Pool.Rent();
                        break;
                    case EActorType.HitBox:
                        logic = APool<HitBoxLogic>.Pool.Rent();
                        break;
                    case EActorType.Loot:
                        logic = APool<LootLogic>.Pool.Rent();
                        break;
                    default:
                        Debug.LogError("非法ActorType");
                        return false;
                }

                actor.Setup(logic);
                return true;
            }
        }

        /// <summary>
        /// 场景功能性对象生成器,该类型对象UID不会变
        /// </summary>
        private static class SceneActorFactory
        {
            public static bool ActorSetup(ref Actor actor, in ActorModel actorModel)
            {
                ActorLogic logic = null;
                switch (actor.ActorType)
                {
                    case EActorType.BattleLevelController:
                        logic = APool<BattleController>.Pool.Rent();
                        break;
                    case EActorType.MonsterGenerator:
                        logic = APool<MonsterGeneratorLogic>.Pool.Rent();
                        break;
                    case EActorType.TriggerBox:
                        logic = APool<TriggerBoxLogic>.Pool.Rent();
                        break;
                    default:
                        Debug.LogError("非法ActorType");
                        return false;
                }

                actor.ModelController.Model = actorModel;
                actor.Setup(logic);
                return true;
            }
        }
    }
}