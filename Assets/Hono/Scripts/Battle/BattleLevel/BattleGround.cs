﻿using System.Collections.Generic;
using Hono.Scripts.Battle.Scene;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        private EBattleStateType _currentStateType;
        private EBattleStateType _nextStateType;
        private BattleState _currentState;
        private readonly string _battleGroundName;
        private readonly Dictionary<EBattleStateType, BattleState> _battleStates;

        private BattleLevelData _levelData;
        private PawnTeamController _pawnTeamController;
        private bool _isScoreSuccess;
        private readonly Dictionary<int, Vector3> _birthPoint = new(BattleConstValue.TeamMaxCount);
        public BattleGroundRtInfo RuntimeInfo { get; }

        public BattleGround(string name)
        {
            _battleGroundName = name;
            _isScoreSuccess = false;
            _pawnTeamController = new PawnTeamController(this);
            RuntimeInfo = new BattleGroundRtInfo();
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
        }

        public void OnCreate() { }

        public void EnterGround() { }

        public void OnDestroy()
        {
            RuntimeInfo.ClearAll();
            //
        }

        private void switchState(EBattleStateType nextStateType)
        {
            _nextStateType = nextStateType;
        }

        public bool TryGetTeamPoint(int teamIdx,out Vector3 centerPos)
        {
            return _birthPoint.TryGetValue(teamIdx, out centerPos);
        }
        
        public void Update(float dt)
        {
            ///临时放置在这里
            ActorManager.Instance.Update(dt);
        }

        public void Tick(float dt)
        {
            ///临时放置在这里
            ActorManager.Instance.Tick(dt);
            _pawnTeamController.Tick(dt);
            
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
    }
}