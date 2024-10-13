using System.Collections.Generic;
using Hono.Scripts.Battle.Scene;

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
        private PawnTeamInfos _pawnTeamInfos;
        private bool _isScoreSuccess;
        private bool _canExit;

        public BattleGround(string name)
        {
            _battleGroundName = name;
            _isScoreSuccess = false;
            _canExit = false;
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
            //
        }

        private void switchState(EBattleStateType nextStateType)
        {
            _nextStateType = nextStateType;
        }

        public void Update(float dt) { }

        public void Tick(float dt)
        {
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