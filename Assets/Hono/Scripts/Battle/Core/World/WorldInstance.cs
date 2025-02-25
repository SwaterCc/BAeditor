using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Core.Base;
using Hono.Scripts.Battle.Core.DerivedLevel;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Tools;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        /// <summary>
        /// 当前关卡
        /// </summary>
        public readonly Level Level;

        //世界的构成
        //静态网格地图数据（可行区域，地图网格对应坐标区域）
        //运行时动态网格数据（网格上的单位数据，寻路数据，单位坐标更新（最后帧））
        private readonly List<IWorldSystemWhenEnterCalled> _mgrsEnter = new();
        private readonly List<IWorldSystemWhenTickCalled> _mgrsTick = new();
        private readonly List<IWorldSystemWhenExitCalled> _mgrsExit = new();

        /// <summary>
        /// 世界单位
        /// </summary>
        public WorldRoot WorldRoot { get; private set; }

        /// <summary>
        /// Id生成器
        /// </summary>
        private readonly CommonUtility.IdGenerator _idGenerator = new();
        /// <summary>
        /// 世界时间缩放系数
        /// </summary>
        private float _worldTimeScale;
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
        private readonly List<Unit> _loadingCaches = new(1024);
        /// <summary>
        /// 加载结束接口
        /// </summary>
        private readonly List<Unit> _loadingFinishList = new(1024);
        /// <summary>
        /// 待删除列表
        /// </summary>
        private readonly List<Unit> _removeList = new(16);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        private AsyncOperation _asyncOperation;
        /// <summary>
        /// 是否加载完成
        /// </summary>
        private bool _isLoadFinish;

        /// <summary>
        /// 当前玩家控制的单位
        /// </summary>
        public Unit PlayerControlUnit { get; private set; }

        /// <summary>
        /// 世界自运行后的持续时长
        /// </summary>
        public float RealWorldTimeSinceStart { get; private set; }

        /// <summary>
        /// 一帧时长
        /// </summary>
        public float OnceTickTime { get; private set; }

        /// <summary>
        /// 世界时间缩放值
        /// </summary>
        public float WorldTimeScale
        {
            get => _worldTimeScale;
            set => _worldTimeScale = Mathf.Max(0, value);
        }

        #region 周期

        public WorldInstance(int sceneTableId)
        {
            register(GPoolManager.Instance);
            register(EventManager.Instance);
            register(MessageManager.Instance);
            register(VFXSystem.Instance);

            _sceneRow = ConfigDataBase.Table<BattleSceneTable>().Get(sceneTableId);

            WorldTimeScale = 1;
            RealWorldTimeSinceStart = 0;

            Query = new WorldQuery(this);
            switch ((ELevelType)_sceneRow.BattleType)
            {
                case ELevelType.Normal:
                    Level = new NormalLevel();
                    break;
                case ELevelType.War:
                    Level = new WarLevel();
                    break;
                default:
#if UNITY_EDITOR
                    Level = new DebugLevel();
#else
                    Level = new EmptyLevel();
#endif
                    break;
            }
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
            //设置单例
            World.SetWorld(this);

            //主动GC一下
            GC.Collect();

            //特定池创建指定数量缓存

            foreach (var system in _mgrsEnter)
            {
                system.OnWorldEnter(this);
            }

            //进入场景
            EnterScene();
        }

        private async void EnterScene()
        {
            try
            {
                //进入加载场景
                await SceneManager.LoadSceneAsync("BattleLoading");

                //异步加载游戏场景
                _asyncOperation = SceneManager.LoadSceneAsync(_sceneRow.ScenePath);
                if (_asyncOperation == null)
                    throw new NullReferenceException($"加载场景{_sceneRow.ScenePath}失败");
                _asyncOperation.allowSceneActivation = false;

                //等待直到场景加载基本完成
                await UniTask.WaitUntil(
                    () => Mathf.Approximately(0.9f, _asyncOperation.progress) && !_asyncOperation.isDone);

                //进入场景
                _asyncOperation.allowSceneActivation = true;

                //加载主UICanvas
                await UIManager.Instance.LoadMainCanvas();

                //创建世界原点
                WorldRoot = new WorldRoot();
                WorldRoot.SetAttr(EAttrType.AttrModelId, 1);
                await UnityAdapter.Instance.CreateUnityObjectProxy(WorldRoot);
                addUnitToWorld(WorldRoot);

                //关卡加载
                await Level.Load();

                _isLoadFinish = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                BattleManager.Instance.ExitWorld();
            }
        }

        /// <summary>
        /// Tick入口
        /// </summary>
        /// <param name="dt"></param>
        public void Tick(float dt)
        {
            //更新一帧时长
            OnceTickTime = dt * _worldTimeScale;
            //更新世界时长
            RealWorldTimeSinceStart += OnceTickTime;

            if (!_isLoadFinish)
            {
                return;
            }

            //关卡更新
            Level.Tick();

            //系统更新
            foreach (var system in _mgrsTick)
            {
                system.OnWorldTick(dt);
            }

            //Unit更新
            UnitsTick();

            //Unit同步
            UnityAdapter.Instance.SyncProxiesTransform();
        }

        /// <summary>
        /// 单位更新
        /// </summary>
        private void UnitsTick()
        {
            //单位更新
            int i = 0;
            while (i < _loadingCaches.Count)
            {
                Unit unit = _loadingCaches[i++];

                if (unit.IsLoadFinish)
                {
                    _loadingFinishList.Add(unit);
                }

                if (unit.HasLoadError)
                {
                    _removeList.Add(unit);
                }
            }

            foreach (var unit in _loadingFinishList)
            {
                _loadingCaches.RemoveSwapBack(unit);
                addUnitToRunningList(unit);
            }

            _loadingFinishList.Clear();

            i = 0;
            while (i < _runningActorList.Count)
            {
                Unit unit = _runningActorList[i++];
                unit.Tick(OnceTickTime);
            }

            if (_removeList is { Count: > 0 })
            {
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
            unit.Init();

            unit.Load();

            if (unit.IsLoadFinish)
            {
                addUnitToRunningList(unit);
                return;
            }

            if (unit.HasLoadError)
            {
                _removeList.Add(unit);
                return;
            }

            _loadingCaches.Add(unit);
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
            return _idGenerator.GenerateId();
        }

        /// <summary>
        /// 创建玩家角色
        /// </summary>
        /// <param name="actorJsonKey"></param>
        /// <returns></returns>
        public Actor CreatePlayerCharacter(string actorJsonKey)
        {
            Actor actor = GPool<Actor>.Pool.Rent();
            try
            {
                ActorJsonAssemblerFactory.Instance.Assemble(actorJsonKey, actor);
                //从外部获取养成数据
                //actro.Attrs.InitAttr();
                addUnitToWorld(actor);
                return actor;
            }
            catch (Exception e)
            {
                GPool<Actor>.Pool.Recycle(actor);
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        ///  创建Actor
        ///  玩家角色(士兵)的属性来自养成转换
        ///  地图其他单位的属性来自静态配置，地图参数，等级影响等
        /// </summary>
        public Actor CreateActor(string actorJsonKey, int faction, Vector3 position, Quaternion rot)
        {
            Actor actor = GPool<Actor>.Pool.Rent();
            try
            {
                actor.UnitTransform.Pos = position;
                actor.UnitTransform.Rot = rot;
                ActorJsonAssemblerFactory.Instance.Assemble(actorJsonKey, actor);
                actor.SetAttr(EAttrType.AttrFaction, faction);
                addUnitToWorld(actor);
                return actor;
            }
            catch (Exception e)
            {
                GPool<Actor>.Pool.Recycle(actor);
                Debug.LogError(e);
                return null;
            }
        }

        public struct InheritSetting
        {
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
        /// <param name="actorJsonKey"></param>
        /// <param name="inheritSetting"></param>
        /// <returns></returns>
        public Actor SummonActor(Actor summoner, string actorJsonKey, InheritSetting inheritSetting)
        {
            Actor actor = GPool<Actor>.Pool.Rent();
            try
            {
                ActorJsonAssemblerFactory.Instance.Assemble(actorJsonKey, actor);
                actor.Attrs.SetSummoned(summoner);
                actor.Attrs.InheritAttrs(summoner, inheritSetting);
                addUnitToWorld(actor);
                return actor;
            }
            catch (Exception e)
            {
                GPool<Actor>.Pool.Recycle(actor);
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// 创建锁目标子弹
        /// </summary>
        public Bullet CreateLockTargetBullet(Unit attacker,
            Unit target,
            BulletData bulletData,
            EDamageSourceType damageSourceType,
            int sourceAbilityId,
            int hitTargetDamageId,
            int hitNotTargetDamageId = 0)
        {
            var bullet = GPool<Bullet>.Pool.Rent();
            bullet.LockTargetBullet(attacker, target, bulletData, damageSourceType, sourceAbilityId, hitTargetDamageId,
                                    hitNotTargetDamageId);
            bullet.Ctor();
            addUnitToWorld(bullet);
            return bullet;
        }

        /// <summary>
        /// 创建方向子弹
        /// </summary>
        public Bullet CreateDirectionBullet(Unit attacker,
            float yAxisAngle,
            BulletData bulletData,
            EDamageSourceType damageSourceType,
            int sourceAbilityId,
            int hitTargetDamageId,
            int hitNotTargetDamageId)
        {
            var bullet = GPool<Bullet>.Pool.Rent();
            bullet.DirectionBullet(attacker, yAxisAngle, bulletData, damageSourceType, sourceAbilityId,
                                   hitTargetDamageId,
                                   hitNotTargetDamageId);
            bullet.Ctor();
            addUnitToWorld(bullet);
            return bullet;
        }

        #region 创建打击盒

        /// <summary>
        /// 创建锁定目标单体打击的打击盒
        /// </summary>
        public void CreateLockTargetSingleHitBox(Unit attacker,
            Unit target,
            EDamageSourceType damageSourceType,
            int sourceAbilityId,
            int maxHitNumber,
            float delayTime,
            float interval,
            bool disableEventTrigger = false,
            int damageId = 0)
        {
            var hitBox = GPool<HitBox>.Pool.Rent();
            hitBox.LockTargetSingleHit(attacker, target, damageSourceType, sourceAbilityId, maxHitNumber, delayTime,
                                       interval, disableEventTrigger, damageId);
            addUnitToWorld(hitBox);
        }

        /// <summary>
        /// 创建锁定目标范围打击的打击盒
        /// </summary>
        public void CreateLockTargetAreaHitBox(Unit attacker,
            Unit target,
            EDamageSourceType damageSourceType,
            int sourceAbilityId,
            int maxHitNumber,
            RangeFilterSetting aoeSetting,
            float delayTime,
            float interval,
            bool disableEventTrigger = false,
            int damageId = 0)
        {
            var hitBox = GPool<HitBox>.Pool.Rent();
            hitBox.LockTargetAreaHit(attacker, target, damageSourceType, sourceAbilityId, maxHitNumber, aoeSetting,
                                     delayTime,
                                     interval, disableEventTrigger, damageId);
            addUnitToWorld(hitBox);
        }

        /// <summary>
        /// 创建锁定目标范围打击的打击盒
        /// </summary>
        public void CreateHitAreaBox(Unit attacker,
            Vector3 worldPos,
            float yAngle,
            EDamageSourceType damageSourceType,
            int sourceAbilityId,
            int maxHitNumber,
            RangeFilterSetting aoeSetting,
            float delayTime,
            float interval,
            bool disableEventTrigger = false,
            int damageId = 0)
        {
            var hitBox = GPool<HitBox>.Pool.Rent();
            hitBox.HitArea(attacker, worldPos, yAngle, damageSourceType, sourceAbilityId, maxHitNumber, aoeSetting,
                           delayTime,
                           interval, disableEventTrigger, damageId);
            addUnitToWorld(hitBox);
        }

        #endregion


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
            //立刻从速查索引中移除
            Query.RemoveUnitLookup(unit);
            //从四叉树中叉出去

            //立刻清理其身上的特效
            VFXSystem.Instance.RemoveUnitAllVFX(unit);
            //回收其坐标
            //回收其位移
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

        /// <summary>
        /// 切换玩家控制单位
        /// </summary>
        public void SwitchPlayerControlUnit(int unitUid)
        {
            if (!Query.TryGetUnit(unitUid, out Unit unit))
            {
                Debug.Log($"找不到要操控的单位 {unit}");
                return;
            }

            if (unit.DisablePlayerControl)
            {
                Debug.Log($"该单位禁止被操控 {unit}");
                return;
            }

            PlayerControlUnit?.SetAttr(EAttrType.AttrIsPlayerCtrl, 0);
            unit.SetAttr(EAttrType.AttrIsPlayerCtrl, 1);
            PlayerControlUnit = unit;
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