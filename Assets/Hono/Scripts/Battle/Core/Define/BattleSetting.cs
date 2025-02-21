#region

using System;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public static class BattleSetting
    {
	    public static readonly string DataRoot = "Assets/BattleData";
	    public static readonly string CSVRoot = $"{DataRoot}/CSV";
        public static readonly string PathFile = $"{DataRoot}/Paths.asset";
        public static readonly string AbilityRoot = $"{DataRoot}/Ability";
        public static readonly string SkillFolder = $"{DataRoot}/Skill";
        public static readonly string BuffFolder = $"{DataRoot}/Buff";
        public static readonly string BulletFolder = $"{DataRoot}/Bullet";
        public static readonly string GameModel = $"{DataRoot}/GameModel";
        public static readonly string PreModelRoot = $"{DataRoot}/GameTestRes/TestModel";
        public static readonly string BattleRootModel = $"{PreModelRoot}/BattleController.prefab";
        public static readonly string BulletModel = $"{PreModelRoot}/BulletModel.prefab";
        public static readonly string HitBoxModel = $"{PreModelRoot}/HitBoxModel.prefab";
        public static readonly string TeamRefreshPoint = $"{PreModelRoot}/TeamRefreshPoint.prefab";
        public static readonly string LootModel = $"{PreModelRoot}/LootModel.prefab";
        
        /// <summary>
        /// 世界节点UID
        /// </summary>
        public const int WorldRootUid = 1;
        
        public static LayerMask ActorPawn = 1 << 9;
        public static LayerMask ActorAlly = 1 << 11;
        public static LayerMask ActorEnemy = 1 << 12;
        public static LayerMask ActorLayerMask = ActorPawn | ActorAlly | ActorEnemy;

        public const int Max_Running_Unit_Count = 5000;
        
        
        public static string GetUOProxyPath(EUOProxyType proxyType)
        {
            switch (proxyType)
            {
                case EUOProxyType.ActorProxy:
                    return "Assets/BattleModel/Prototype/UOProxy/ActorProxy.prefab";
                case EUOProxyType.TransformProxy:
                    return "Assets/BattleModel/Prototype/UOProxy/TransformProxy.prefab";
                case EUOProxyType.CubeColliderProxy:
                    return "Assets/BattleModel/Prototype/UOProxy/CubeColliderProxy.prefab";
                case EUOProxyType.SphereColliderProxy:
                    return "Assets/BattleModel/Prototype/UOProxy/SphereColliderProxy.prefab";
                case EUOProxyType.CapsuleColliderProxy:
                    return "Assets/BattleModel/Prototype/UOProxy/CapsuleColliderProxy.prefab";
                case EUOProxyType.CubeTriggerProxy:
                    return "Assets/BattleModel/Prototype/UOProxy/CubeTriggerProxy.prefab";
                case EUOProxyType.CylinderTriggerProxy:
                    return "Assets/BattleModel/Prototype/UOProxy/CylinderTriggerProxy.prefab";
                default:
                    throw new ArgumentOutOfRangeException(nameof(proxyType), proxyType, null);
            }
            return null;
        }
        
        public static LayerMask GetLayerMask(string layerType)
        {
            switch (layerType)
            {
              case "SceneUnit":
                  return 7;
              case "Character":
                  return 9;
              case "Ally":
                  return 10;
              case "Enemy":
                  return 11;
            }

            return 7;
        }
    }
}