using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Hono.Scripts.Battle.Scene;
using Hono.Scripts.Battle.Tools;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public interface ITick
    {
        public void Tick(float dt);
    }

    public class BattleRoot : MonoSingleton<BattleRoot>
    {
        private static BattleRoot _instance;
        private bool _isFirstUpdate;
        private bool _hasError;

        private Actor _battleMode;
        public static Actor BattleModeActor => Instance._battleMode;
        public static BattleLevelControl BattleLevelControl => (BattleLevelControl)Instance._battleMode.Logic;

        private BattleLevelData _battleLevelData;
        public BattleLevelData BattleLevelData => _battleLevelData;

        public int GroupCount { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            init();
        }

        private void init()
        {
            _isFirstUpdate = true;
            //初始化Lua环境
            LuaInterface.Init();
            //反射缓存
            AbilityFuncPreLoader.InitAbilityFuncCache();
#if UNITY_EDITOR
            DebugMode.Instance.Init();
#endif
            //CSV加载
            ConfigManager.Instance.Init();
            //Asset资源加载
            AssetManager.Instance.Init();
            //初始化ActorManager
            ActorManager.Instance.Init();

            //装载关卡数据
            setupBattleLevelData();
        }

        private void setupBattleLevelData()
        {
            
        }

        /// <summary>
        /// 创建战场对象
        /// </summary>
        private void initBattleMode()
        {
            _battleMode = ActorManager.Instance.GetBattleMode();
            _battleMode.Init();
        }

        /// <summary>
        /// 创建玩家Group
        /// </summary>
        private bool initPawnGroup()
        {
            for (int idx = 0; idx < BattleConstValue.PawnGroupMaxCount; idx++)
            {
                var groupInfo = _battleLevelData.GroupInfos[idx];
                if (groupInfo.IsEmpty()) continue;
                var pawnStartPoint = _battleLevelData.GroupStartPoint[idx];
                if (pawnStartPoint == null) continue;

                bool hasLeader = false;
                var groupState = new PawnGroupState(idx);
                for (int memberIdx = 0; memberIdx < BattleConstValue.PawnGroupMemberMaxCount; memberIdx++)
                {
                    if(groupInfo[memberIdx] <= 0) continue;
                    var pawnActor = ActorManager.Instance.CreateActor(EActorType.Pawn, groupInfo[memberIdx]);
                    var pawnLogic = (PawnLogic)pawnActor.Logic;
                    pawnLogic.GroupMemberIdx = memberIdx;
                    pawnLogic.BelongActorGroupId = groupInfo.GroupId;
                    groupState[memberIdx].Init(pawnActor.Uid,pawnActor.ConfigId,EPawnGroupMemberStateType.Normal);
                    ActorManager.Instance.AddActor(pawnActor);
                    if (!hasLeader)
                    {
                        groupState[memberIdx].IsLeader = true;
                        hasLeader = true;
                    }
                }

                if (groupState.MemberCount > 0)
                {
                    ++GroupCount;
                    PawnGroupManager.Instance.AddGroup(groupState.GroupIndex, groupState);
                }
            }

            return GroupCount > 0;
        }

        private void initStaticActor()
        {
            foreach (var actorRefreshPoint in _battleLevelData.StaticActor)
            {
                actorRefreshPoint.CreateActor();
            }
        }

        private void battleLevelBuild()
        {
            initBattleMode();

            if (!initPawnGroup())
            {
                _hasError = true;
                Debug.LogError("初始化玩家角色失败");
            }

            initStaticActor();

            _isFirstUpdate = false;
        }

        public void Update()
        {
            if (!AssetManager.Instance.IsLoadFinish || !ConfigManager.Instance.IsLoadFinish)
            {
                return;
            }

            if (_battleLevelData == null)
            {
                Debug.LogError("找不到场景数据 应当存在名为 BattleLevelData的GameObject对象");
                return;
            }

            if (_isFirstUpdate)
            {
                battleLevelBuild();
            }

            if (_hasError)
            {
                return;
            }

            //临时做法
            //保证逻辑帧在表现帧之前执行一次
            Tick(Time.deltaTime);

            _battleMode.Update(Time.deltaTime);
            ActorManager.Instance.Update(Time.deltaTime);
        }

        public void Tick(float dt)
        {
            _battleMode.Tick(Time.deltaTime);
            ActorManager.Instance.Tick(dt);
            MessageCenter.Instance.Tick(dt);
            PawnGroupManager.Instance.Tick(dt);
        }

        public void OnDestroy()
        {
            LuaInterface.Dispose();
        }
    }
}