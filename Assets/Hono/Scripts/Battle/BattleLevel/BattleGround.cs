#region

using System.Collections.Generic;
using Hono.Scripts.Battle.Scene;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        private EBattleStateType _currentStateType;
        private EBattleStateType _nextStateType;
        private BattleState _currentState;
        private readonly string _battleGroundName;
        private readonly Dictionary<EBattleStateType, BattleState> _battleStates;
        private BattleController _battleController;
        private BattleLevelData _levelData;
        private readonly PawnTeamController _pawnTeamController;
        private readonly LootController _lootController;
        private bool _isScoreSuccess;
        private readonly Dictionary<int, Vector3> _birthPoint = new(BattleConstValue.TeamMaxCount);
        private readonly Dictionary<int, int> _teamRefreshPoint = new(BattleConstValue.TeamMaxCount);
        public BattleGroundRtInfo RtInfo { get; }
        public int BattleGroundConfigId { get; }
        public BattleLevelData LevelData => _levelData;
        public PawnTeamController TeamController => _pawnTeamController;
        public LootController LootController => _lootController;

        public BattleSceneTable.BattleSceneRow BattleConfig { get; }
        public bool Result => _isScoreSuccess;
        public BattleController BattleController => _battleController;

        public BattleGround(int configId)
        {
            BattleGroundConfigId = configId;
            BattleConfig = ConfigManager.Table<BattleSceneTable>().Get(configId);

            _battleGroundName = BattleConfig.ScenePath;
            _isScoreSuccess = false;
            _pawnTeamController = new PawnTeamController(this);
            _lootController = new LootController();
            RtInfo = new BattleGroundRtInfo();
            _battleStates = new Dictionary<EBattleStateType, BattleState>()
            {
                { EBattleStateType.NoGaming, new NoGameState(this, EBattleStateType.NoGaming) },
                { EBattleStateType.BuildTeams, new BuildTeamsState(this, EBattleStateType.BuildTeams) },
                {
                    EBattleStateType.LoadBattleGround,
                    new LoadBattleGroundState(this, EBattleStateType.LoadBattleGround)
                },
                { EBattleStateType.Playing, new GameRunningState(this, EBattleStateType.Playing) },
                { EBattleStateType.Score, new ScoreState(this, EBattleStateType.Score) },
            };

            _currentStateType = EBattleStateType.NoGaming;
            _currentState = _battleStates[_currentStateType];
        }

        public void OnCreate() { }

        public void EnterGround()
        {
            switchState(EBattleStateType.LoadBattleGround);
            MonsterGeneratorLogic.CurMonsterCount = 0;
        }

        public void OnDestroy()
        {
            RtInfo.ClearAll();
            ActorManager.Instance.ClearAllActor();

            MonsterGeneratorLogic.CurMonsterCount = 0;
        }

        public bool TryGetSaveFile(out BattleSaveFile saveFile)
        {
            saveFile = null;
            return false;
        }

        private void switchState(EBattleStateType nextStateType)
        {
            //Debug.Log($"[BattleState] switchState Next {nextStateType}");
            _nextStateType = nextStateType;
        }

        public bool TryGetTeamPoint(int teamIdx, out Vector3 centerPos)
        {
            if (!_teamRefreshPoint.TryGetValue(teamIdx, out var pointUid) || pointUid <= 0)
            {
                return _birthPoint.TryGetValue(teamIdx, out centerPos);
            }

            if (!ActorManager.Instance.TryGetActor(pointUid, out var point))
            {
                return _birthPoint.TryGetValue(teamIdx, out centerPos);
            }

            centerPos = point.Pos;
            return true;
        }

        public void AddTeamRefreshPoint(int teamIdx, int uid)
        {
            if (!_teamRefreshPoint.TryGetValue(teamIdx, out var pointUid) || pointUid <= 0)
            {
                _teamRefreshPoint.Add(teamIdx, uid);
            }
            else
            {
                _teamRefreshPoint[teamIdx] = uid;
                ActorManager.Instance.RemoveActor(pointUid);
            }
        }

        public void Tick(float dt)
        {
            ActorManager.Instance.Tick(dt);

            if (_currentState == null) return;

            _currentState.Tick(dt);

            if (_currentStateType != _nextStateType)
            {
                _currentState.Exit();
                _currentState = _battleStates[_nextStateType];
                _currentStateType = _nextStateType;
                _currentState.Enter();
            }
        }

        public void RoundBegin()
        {
            if (_currentState.StateType != EBattleStateType.Playing)
            {
                return;
            }

            ((GameRunningState)_currentState).RoundBegin();
        }

        public void BuildTeam(PawnTeamDataList dataList)
        {
            _pawnTeamController.BuildTeam(dataList);
        }
    }
}