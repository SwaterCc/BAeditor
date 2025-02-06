using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Base;
using Hono.Scripts.Battle.Event;
using UnityEngine;

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

    public interface IWorldSystem { }

    public interface IWorldSystemWhenEnterCalled : IWorldSystem
    {
        public void OnWorldEnter(World world);
    }

    public interface IWorldSystemWhenTickCalled : IWorldSystem
    {
        public void OnWorldTick(float dt);
    }

    public interface IWorldSystemWhenExitCalled : IWorldSystem
    {
        public void OnWorldExit();
    }

    /// <summary>
    /// 当前运行的世界
    /// </summary>
    public partial class World
    {
        /// <summary>
        /// 世界最大Actor数量
        /// </summary>
        public const int MaxActorCount = 2000;
        /// <summary>
        /// 最大Unit的数量
        /// </summary>
        public const int MaxUnitCount = MaxActorCount + 3000;

        //世界的构成
        //静态网格地图数据（可行区域，地图网格对应坐标区域）
        //运行时动态网格数据（网格上的单位数据，寻路数据，单位坐标更新（最后帧））
        private readonly List<IWorldSystemWhenEnterCalled> _mgrsEnter = new();
        private readonly List<IWorldSystemWhenTickCalled> _mgrsTick = new();
        private readonly List<IWorldSystemWhenExitCalled> _mgrsExit = new();
        /// <summary>
        /// 根节点
        /// </summary>
        private readonly WorldNodeRoot _worldNodeRoot = new();

        /// <summary>
        /// Unit搜索器
        /// </summary>
        public readonly UnitSearcher Searcher = new();

        /// <summary>
        /// 当前世界流程
        /// </summary>
        private EWorldState _currentState;

        /// <summary>
        /// 下一个世界流程
        /// </summary>
        private EWorldState _nextState;

        /// <summary>
        /// 状态集合
        /// </summary>
        private Dictionary<EWorldState, WorldState> _worldStates;

        /// <summary>
        /// 场景数据Id(场景id，静态地图网格)
        /// </summary>
        private int _worldSceneKey;

        /// <summary>
        /// 世界对象数据（触发器，场景对象，场景事件，场景流程）
        /// </summary>
        private int _worldInfoKey;

        /// <summary>
        /// 数据配置
        /// </summary>
        private BattleSceneTable.BattleSceneRow _sceneRow;

        #region 周期

        public World(int sceneTableId)
        {
            register(HitManager.Instance);
            register(EventManager.Instance);
            register(MessageManager.Instance);

            _sceneRow = ConfigManager.Table<BattleSceneTable>().Get(sceneTableId);

            _worldStates = new Dictionary<EWorldState, WorldState>()
            {
                { EWorldState.Loading, new LoadingState(this) },
                { EWorldState.Ready, new ReadyState(this) },
                { EWorldState.Gaming, new GamingState(this) },
                { EWorldState.StrategicMap, new StrategicMapState(this) },
                { EWorldState.Score, new ScoreState(this) },
            };

            _currentState = _nextState = EWorldState.NoInit;
        }

        private void register(IWorldSystem system)
        {
            if (system is IWorldSystemWhenEnterCalled enterCalled && !_mgrsEnter.Contains(enterCalled))
            {
                _mgrsEnter.Add(enterCalled);
            }

            if (system is IWorldSystemWhenTickCalled tickCalled && !_mgrsTick.Contains(tickCalled))
            {
                _mgrsTick.Add(tickCalled);
            }

            if (system is IWorldSystemWhenExitCalled exitCalled && !_mgrsExit.Contains(exitCalled))
            {
                _mgrsExit.Add(exitCalled);
            }
        }

        /// <summary>
        /// 启动！
        /// </summary>
        public void Enter()
        {
            //主动GC一下
            GC.Collect();

            //特定池创建指定数量缓存
            
            foreach (var system in _mgrsEnter)
            {
                system.OnWorldEnter(this);
            }

            //进入加载状态
            _nextState = EWorldState.Loading;
        }

        /// <summary>
        /// Tick入口
        /// </summary>
        /// <param name="dt"></param>
        public void Tick(float dt)
        {
            if (_currentState != _nextState)
            {
                _worldStates[_currentState]?.Exit();
                _worldStates[_nextState]?.Enter(_currentState);
                _currentState = _nextState;
            }

            _worldStates[_currentState]?.Tick(dt);
            
            foreach (var system in _mgrsTick)
            {
                system.OnWorldTick(dt);
            }
        }

        /// <summary>
        /// 离开世界
        /// </summary>
        public void Exit()
        {
            foreach (var system in _mgrsExit)
            {
                system.OnWorldExit();
            }

            //退出后主动GC下
            GC.Collect();
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

        public void OpenStrategicMap()
        {
            if (_currentState == EWorldState.Gaming && (EBattleModeType)_sceneRow.BattleType == EBattleModeType.War)
            {
                _nextState = EWorldState.StrategicMap;
            }
        }

        public void CloseStrategicMap()
        {
            if (_currentState == EWorldState.StrategicMap)
            {
                _nextState = EWorldState.Gaming;
            }
        }
    }

    /// <summary>
    /// 世界状态
    /// </summary>
    public partial class World
    {
        private abstract class WorldState
        {
            public readonly EWorldState State;
            public readonly World World;

            protected WorldState(World world, EWorldState worldState)
            {
                State = worldState;
                World = world;
            }

            public void Enter(EWorldState beforeState)
            {
                Debug.Log($"[WorldState] before {beforeState}  Switch To {State}");
                OnEnter();
            }

            protected abstract void OnEnter();

            public void Tick(float dt)
            {
                OnTick(dt);
            }

            protected abstract void OnTick(float dt);

            public void Exit()
            {
                OnExit();
            }

            protected abstract void OnExit();
        }
    }
}