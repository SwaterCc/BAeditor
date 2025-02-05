using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;

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

    public interface IWorldSystem
    {
        public void EnterWorld();
        public void Tick(float dt);
        public void ExitWorld();
    }

    /// <summary>
    /// 当前运行的世界
    /// </summary>
    public partial class World
    {
        /// <summary>
        /// 世界最大Actor数量
        /// </summary>
        private const int MaxActorCount = 1000;
        
        //世界的构成
        //静态网格地图数据（可行区域，地图网格对应坐标区域）
        //运行时动态网格数据（网格上的单位数据，寻路数据，单位坐标更新（最后帧））
        private readonly Dictionary<Type, IWorldSystem> _systems = new()
        {
            { typeof(EventManager), new EventManager() },
            { typeof(MessageManager), new MessageManager() },
        };

        /// <summary>
        /// 根节点
        /// </summary>
        private readonly WorldNodeRoot _worldNodeRoot = new();

        /// <summary>
        /// Actor搜索器
        /// </summary>
        public readonly ActorSearcher Searcher = new();

        /// <summary>
        /// 当前世界流程
        /// </summary>
        private EWorldState _state;

        /// <summary>
        /// 场景数据Id(场景id，静态地图网格)
        /// </summary>
        private int _worldSceneKey;

        /// <summary>
        /// 世界对象数据（触发器，场景对象，场景事件，场景流程）
        /// </summary>
        private int _worldInfoKey;
        
        #region 周期

        /// <summary>
        /// 启动！
        /// </summary>
        public void Start()
        {
            //地图网格初始化
            //搜索器初始化
            foreach (var system in _systems.Values)
            {
                system.EnterWorld();
            }
            
            //进入加载状态
            _state = EWorldState.Loading;
        }

        /// <summary>
        /// Tick入口
        /// </summary>
        /// <param name="dt"></param>
        public void Tick(float dt)
        {
            switch (_state)
            {
                case EWorldState.Loading:
                    
                    break;
                case EWorldState.Process1:
                    
                    break;
                case EWorldState.Process2_1:
                    //
                    break;
                case EWorldState.Process2_2:
                    
                    break;
                case EWorldState.Score:
                    break;
            }
            
            _worldNodeRoot.Tick(dt);
            foreach (var system in _systems.Values)
            {
                system.Tick(dt);
            }
        }

        /// <summary>
        /// 离开世界
        /// </summary>
        public void Exit()
        {
            foreach (var system in _systems.Values)
            {
                system.ExitWorld();
            }
        }

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
        /// 创建子弹
        /// </summary>
        public Bullet CreateBullet(Actor attacker)
        {
            return null;
        }
    }
}