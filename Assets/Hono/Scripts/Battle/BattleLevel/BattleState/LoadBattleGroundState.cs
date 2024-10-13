using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        public class LoadBattleGroundState : BattleState
        {
            private AsyncOperation _asyncOperation;

            public LoadBattleGroundState(BattleGround battleGround, EBattleStateType stateType) : base(battleGround,
                stateType) { }

            protected override async void onEnter()
            {
                SceneManager.LoadScene(0);

                _asyncOperation = SceneManager.LoadSceneAsync(BattleGroundHandle._battleGroundName);
                _asyncOperation.allowSceneActivation = false;
            }

            protected override void onTick(float dt)
            {
                if (_asyncOperation.isDone)
                {
                    setupScene();
                    
                    
                    
                    
                    BattleGroundHandle.switchState(EBattleStateType.Playing);
                }
            }

            private async void setupScene()
            {
                
            }
            
            protected override void onExit()
            {
                _asyncOperation = null;
            }
        }
    }
}