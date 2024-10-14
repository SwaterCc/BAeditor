using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hono.Scripts.Battle
{
    public partial class BattleGround
    {
        public class LoadBattleGroundState : BattleState
        {
            private AsyncOperation _asyncOperation;
            private bool _isSetupScene;
            
            public LoadBattleGroundState(BattleGround battleGround, EBattleStateType stateType) : base(battleGround,
                stateType) { }

            protected override async void onEnter()
            {
                SceneManager.LoadScene(0);
                _isSetupScene = false;
                _asyncOperation = SceneManager.LoadSceneAsync(BattleGroundHandle._battleGroundName);
                _asyncOperation.allowSceneActivation = false;
            }

            protected override void onTick(float dt)
            {
                if (!_asyncOperation.isDone) return;
                
                if (!_isSetupScene)
                    setupScene();
                
                if(BattleGroundHandle._pawnTeamController.PawnLoadFinish)
                    BattleGroundHandle.switchState(EBattleStateType.Playing);
            }

            private void setupScene()
            {
                _isSetupScene = true;
                
                //第一时间把战场控制的Model放到Scene中
                BattleManager.BattleController.ModelController.OnEnterBattleGroundFirstTime();

                //加载BattleLevelData
                BattleGroundHandle._levelData = Object.FindObjectOfType<BattleLevelData>();

                //创建场景Actor
                foreach (var sceneActorModel in BattleGroundHandle._levelData.SceneActorModels)
                {
                    ActorManager.Instance.CreateSceneActor(sceneActorModel, 0, initPawnDefaultBirthPoint);
                }
                
                BattleGroundHandle._pawnTeamController.CreatePawnTeams();
            }

            //收集出生点
            private void initPawnDefaultBirthPoint(Actor actor)
            {
                if (actor.ActorType == EActorType.TeamDefaultBirthPoint)
                {
                    var teamId = (int)actor.Variables.Get("TeamId");
                    BattleGroundHandle._birthPoint.TryAdd(teamId, actor.Pos);
                }
            }

            protected override void onExit()
            {
                _asyncOperation = null;
            }
        }
    }
}