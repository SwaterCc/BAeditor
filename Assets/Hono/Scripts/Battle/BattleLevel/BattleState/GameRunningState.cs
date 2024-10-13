using UnityEngine;

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        public class GameRunningState : BattleState
        {
            private readonly RoundController _roundController;
            private bool _roundInitFlag;

            public GameRunningState(BattleGround battleGroundHandle, EBattleStateType stateType) : base(
                battleGroundHandle, stateType)
            {
                _roundController = new RoundController(this);
            }
           
            protected override void onEnter()
            {
                _roundInitFlag = _roundController.InitRoundController(BattleGroundHandle._levelData.RoundDatas,
                    BattleGroundHandle._levelData.CanRepeatRound);
                if (!_roundInitFlag)
                {
                    Debug.LogError("回合数据不对，启动失败");
                }
            }

            protected override void onTick(float dt)
            {
                if (_roundController.CurrentState == ERoundState.NoRunning)
                {
                    _roundController.SwitchState(ERoundState.Ready);
                }
                
                _roundController.Tick(dt);
                ActorManager.Instance.Tick(dt);
                ActorManager.Instance.Update(dt);
            }

            public void ScoreBattle(bool isPass)
            {
                BattleGroundHandle._isScoreSuccess = isPass;
                BattleGroundHandle.switchState(EBattleStateType.Score);
            }

            protected override void onExit()
            {
                _roundController.SwitchState(ERoundState.NoRunning);
            }
        }
    }
}