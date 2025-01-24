using System.Collections.Generic;
using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle.Core
{
    public class WorldNodeRoot : WorldNode
    {
        public WorldNodeRoot()
        {
            SetRoot(this);
        }

        private new void SetParent(WorldNode parent) { }

        protected override void onTick(float dt) { }

        protected override void onRemove() { }
    }

    /// <summary>
    /// 当前运行的世界
    /// </summary>
    public class World
    {
        //世界的构成
        //网格地图数据
        //地图管理，单位坐标更新，打击点结算，网格运行时数据更新
        //节点管理，

        public WorldNodeRoot Root { get; private set; }

        /// <summary>
        /// 世界级下的节点
        /// </summary>
        private readonly List<WorldNode> _nodes = new(1000);

        #region 周期

        /// <summary>
        /// 启动！
        /// </summary>
        public void Start()
        {
            Root = new WorldNodeRoot();
        }

        /// <summary>
        /// Tick入口
        /// </summary>
        /// <param name="dt"></param>
        public void Tick(float dt)
        {
            Root.Tick(dt);

            //架构更新

            //
        }

        /// <summary>
        /// 离开世界
        /// </summary>
        public void Exit() { }

        #endregion

        /// <summary>
        /// 创建Actor
        /// </summary>
        public Actor CreateActor(int actorTableId, AttrCollection.AttrSnapshots snapshots, WorldNode parent)
        {
            Actor actor = new();
            var uid = UnitUidGenerator.GenerateUid(EUnitUidRangeType.NormalActor);
            actor.Init(uid, actorTableId, null, snapshots);
            return actor;
        }

        /// <summary>
        /// 召唤Actor
        /// </summary>
        public Actor SummonActor(Actor summoner, int actorTableId, string rule, bool fromTopSummer, WorldNode parent)
        {
            Actor summoned = new();
            var uid = UnitUidGenerator.GenerateUid(EUnitUidRangeType.NormalActor);
            summoned.Init(uid, actorTableId, null, summoner.Attrs.GetAttrSnapShots(rule));
            summoned.Attrs.InitSummonedAttrs(summoner.Attrs, fromTopSummer);
            return summoned;
        }

        /// <summary>
        /// 创建打击盒子
        /// </summary>
        public HitBox CreateHitBox(Actor attacker, HitBoxData data)
        {
            return null;
        }

        /// <summary>
        /// 创建子弹
        /// </summary>
        public Bullet CreateBullet(Actor attacker)
        {
            return null;
        }
    }
}