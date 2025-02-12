using System;
using System.Collections.Generic;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Event;
using Unity.Collections;
using UnityEngine;

namespace Hono.Scripts.Battle.Core
{
    public interface IWorldSystem { }

    public interface IWorldSystemWhenEnterCalled : IWorldSystem
    {
        public void OnWorldEnter(WorldInstance worldInstance);
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
    /// 语法糖，快速访问当前World
    /// </summary>
    public static class World
    {
        private static WorldInstance _instance;
        public static WorldInstance Current => _instance;
        public static WorldQuery Query => _instance.Query;

        public static void SetWorld(WorldInstance instance)
        {
            if (instance != null)
            {
                _instance = instance;
            }
        }
    }

    /// <summary>
    /// 世界实例对象
    /// </summary>
    public partial class WorldInstance
    {
        /// <summary>
        /// 世界最大Actor数量
        /// </summary>
        public const int MaxActorCount = 2000;
        /// <summary>
        /// 最大Unit的数量
        /// </summary>
        public const int MaxUnitCount = MaxActorCount + 3000;
        
        /// <summary>
        /// 搜索器,集合了查找过滤的API
        /// </summary>
        public readonly WorldQuery Query;

        //世界的构成
        //静态网格地图数据（可行区域，地图网格对应坐标区域）
        //运行时动态网格数据（网格上的单位数据，寻路数据，单位坐标更新（最后帧））
        private readonly List<IWorldSystemWhenEnterCalled> _mgrsEnter = new();
        private readonly List<IWorldSystemWhenTickCalled> _mgrsTick = new();
        private readonly List<IWorldSystemWhenExitCalled> _mgrsExit = new();

        /// <summary>
        /// 世界单位
        /// </summary>
        private readonly WorldInstanceUnit _worldNodeRoot = new();

        /// <summary>
        /// Id生成器
        /// </summary>
        private readonly IdAllocator _idAllocator = new();

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
        private readonly Dictionary<EWorldState, WorldState> _worldStates;

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
        private readonly BattleSceneTable.BattleSceneRow _sceneRow;

        /// <summary>
        /// 正在运行的Unit列表
        /// </summary>
        private readonly List<Unit> _runningActorList = new(2048);

        /// <summary>
        /// Actor待加载列表
        /// </summary>
        private readonly List<Unit.ILoadableUnit> _loadingCaches = new(1024);

        /// <summary>
        /// 待删除列表
        /// </summary>
        private readonly List<Unit> _removeList = new(16);

        #region 周期

        public WorldInstance(int sceneTableId)
        {
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

            Query = new WorldQuery(this);
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

            //设置单例
            World.SetWorld(this);

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
            //世界更新
            if (_currentState != _nextState)
            {
                _worldStates[_currentState]?.Exit();
                _worldStates[_nextState]?.Enter(_currentState);
                _currentState = _nextState;
            }

            _worldStates[_currentState]?.Tick(dt);

            //系统更新
            foreach (var system in _mgrsTick)
            {
                system.OnWorldTick(dt);
            }

            //单位更新
            int i = 0;
            while (i < _loadingCaches.Count)
            {
                Unit.ILoadableUnit loadable = _loadingCaches[i++];

                if (loadable.IsLoadFinish)
                {
                    _runningActorList.Add((Unit)loadable);
                }

                if (loadable.HasLoadError)
                {
                    _removeList.Add((Unit)loadable);
                }
            }

            i = 0;
            while (i < _loadingCaches.Count)
            {
                Unit unit = _runningActorList[i++];
                unit.Tick(dt);
            }

            if (_removeList is not { Count: > 0 })
                return;

            foreach (var unit in _removeList)
            {
                //回收
                unit.Recycle();
                //父类清理
                unit.BaseClear();
                //从运行队列删除
                _runningActorList.RemoveSwapBack(unit);
            }

            _removeList.Clear();
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

            //设置单例
            World.SetWorld(null);

            //退出后主动GC下
            GC.Collect();
        }

        #endregion

        #region Unit接口

        /// <summary>
        /// Unit添加到世界
        /// </summary>
        private void addUnitToWorld(Unit unit)
        {
            if (unit is Unit.ILoadableUnit loadableUnit)
            {
                loadableUnit.Load();

                if (loadableUnit.IsLoadFinish)
                {
                    addUnitToRunningList(unit);
                    return;
                }

                if (loadableUnit.HasLoadError)
                {
                    _removeList.Add(unit);
                    return;
                }

                _loadingCaches.Add(loadableUnit);
            }
            else
            {
                addUnitToRunningList(unit);
            }
        }

        /// <summary>
        /// 添加到运行队列
        /// </summary>
        /// <param name="unit"></param>
        private void addUnitToRunningList(Unit unit)
        {
            if (!Query.TryAddUnitLookup(unit))
            {
                throw new Exception("UID重复！！！！！");
            }

            _runningActorList.Add(unit);
        }

        /// <summary>
        /// 获取这个World唯一Id
        /// </summary>
        /// <returns></returns>
        public int GetUid()
        {
            return _idAllocator.Allocate();
        }

        /// <summary>
        /// 创建玩家角色
        /// </summary>
        /// <param name="actorTableId"></param>
        /// <returns></returns>
        public Actor CreatePlayerCharacter(int actorTableId)
        {
            if (!ConfigManager.Table<ActorTable>().TryGet(actorTableId, out var row))
            {
                return null;
            }

            Actor actor = ActorPool.Instance.Get(row.PrototypeJsonName);
            //设置初始值
            actor.Attrs.Init(actorTableId);
            //从外部获取养成数据
            //actro.Attrs.InitAttr();
            actor.Init(row);
            addUnitToWorld(actor);
            return actor;
        }

        /// <summary>
        ///  创建Actor
        ///  玩家角色(士兵)的属性来自养成转换
        ///  地图其他单位的属性来自静态配置，地图参数，等级影响等
        /// </summary>
        public Actor CreateActor(int actorTableId)
        {
            if (!ConfigManager.Table<ActorTable>().TryGet(actorTableId, out var row))
            {
                return null;
            }

            Actor actor = ActorPool.Instance.Get(row.PrototypeJsonName);
            actor.Attrs.Init(actorTableId);
            actor.Init(row);
            addUnitToWorld(actor);
            return actor;
        }

        public struct SummonSetting
        {
            public bool FromTopSummer;
            public bool LifeWithSummoner;
            public string Rule;
            public int Param1;
            public int Param2;
            public int Param3;
            public int Param4;
        }

        /// <summary>
        /// 召唤Actor
        /// </summary>
        /// <param name="summoner"></param>
        /// <param name="actorTableId"></param>
        /// <param name="summonSetting"></param>
        /// <returns></returns>
        public Actor SummonActor(Actor summoner, int actorTableId, SummonSetting summonSetting)
        {
            if (!ConfigManager.Table<ActorTable>().TryGet(actorTableId, out var row))
            {
                return null;
            }

            Actor actor = ActorPool.Instance.Get(row.PrototypeJsonName);

            actor.Attrs.Init(actorTableId);
            actor.Attrs.SetSummoned(summoner, summonSetting.FromTopSummer);
            actor.Attrs.InheritAttrs(summoner.Attrs, summonSetting);
            actor.Init(row);
            addUnitToWorld(actor);

            return actor;
        }

        /// <summary>
        /// 创建子弹
        /// </summary>
        public Bullet CreateBullet(Unit attacker)
        {
            var bullet = GPool<Bullet>.Pool.Rent();
            //属性全拷贝X 直接拥有攻击者对象
            addUnitToWorld(bullet);
            return bullet;
        }

        /// <summary>
        /// 创建脱手打击盒
        /// </summary>
        public HitBox CreateHitBox(Unit attacker)
        {
            var hitBox = GPool<HitBox>.Pool.Rent();
            addUnitToWorld(hitBox);
            return hitBox;
        }

        /// <summary>
        /// 删除Unit
        /// </summary>
        /// <param name="unit"></param>
        public void RemoveUnit(Unit unit)
        {
            if (!Query.ContainsUnit(unit.Uid))
                return;
            //加入删除队列
            _removeList.Add(unit);
            //立刻从搜索队列中移除
            Query.RemoveUnitLookup(unit);
        }

        /// <summary>
        /// 删除Unit
        /// </summary>
        /// <param name="unitUid"></param>
        public void RemoveUnit(int unitUid)
        {
            if (!Query.TryGetUnit(unitUid, out var unit))
                return;
            //加入删除队列
            _removeList.Add(unit);
            //立刻从搜索队列中移除
            Query.RemoveUnitLookup(unit);
        }

        #endregion


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
    public partial class WorldInstance
    {
        private abstract class WorldState
        {
            public readonly EWorldState State;
            public readonly WorldInstance WorldInstance;

            protected WorldState(WorldInstance worldInstance, EWorldState worldState)
            {
                State = worldState;
                WorldInstance = worldInstance;
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