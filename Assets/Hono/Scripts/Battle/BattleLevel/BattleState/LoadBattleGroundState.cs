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
            private float _timeCounting;
            
            public LoadBattleGroundState(BattleGround battleGround, EBattleStateType stateType) : base(battleGround,
                stateType) { }

            protected override void onEnter()
            {
                SceneManager.LoadScene(1);
                _isSetupScene = false;
                _asyncOperation = SceneManager.LoadSceneAsync(BattleGroundHandle._battleGroundName);
                _asyncOperation.allowSceneActivation = false;
            }

            protected override void onTick(float dt)
            {
                if (!_asyncOperation.isDone) return;
                
                if (!_isSetupScene)
                    setupScene();

                Debug.Log($"[setupSceneTime] {Time.realtimeSinceStartup - _timeCounting }");
                BattleGroundHandle.switchState(EBattleStateType.Playing);
            }

            private void setupScene()
            {
                _isSetupScene = true;
                _timeCounting = Time.realtimeSinceStartup;
                
                //第一时间把战场控制的Model放到Scene中
                BattleManager.BattleController.ModelController.OnEnterBattleGroundFirstTime();

                //加载BattleLevelData
                BattleGroundHandle._levelData = Object.FindObjectOfType<BattleLevelData>();
                if (BattleGroundHandle == null)
                {
                    Debug.LogError("当前场景缺少BattleLevelData");
                    return;
                }
                
                //创建场景Actor
                foreach (var sceneActorModel in BattleGroundHandle._levelData.SceneActorModels)
                {
                    ActorManager.Instance.CreateSceneActor(sceneActorModel, 0, initPawnDefaultBirthPoint);
                }
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