#region

using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public static class BattleConstValue
    {
        public static readonly string DataRoot = "Assets/BattleData";

        public static readonly string PathFile = $"{DataRoot}/Paths.asset";
        public static readonly string CSVRoot = $"{DataRoot}/CSV";

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
        public static readonly string DefaultModel = "";
        
        public const int BattleRootControllerUid = 1;

        /// <summary>
        ///     游戏中最大队伍数量
        /// </summary>
        public static int TeamMaxCount = 4;

        /// <summary>
        ///     队伍成员最大数量
        /// </summary>
        public static int TeamMemberMaxCount = 4;


        public static LayerMask ActorPawn = 1 << 10;
        public static LayerMask ActorAlly = 1 << 11;
        public static LayerMask ActorEnemy = 1 << 12;

        public static LayerMask ActorLayerMask = ActorPawn | ActorAlly | ActorEnemy;
    }
}