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
                 loadingScene();
                 _timeCounting = Time.realtimeSinceStartup;
            }

            private async void loadingScene() {
	            await SceneManager.LoadSceneAsync(1);
	            _asyncOperation = SceneManager.LoadSceneAsync(BattleGroundHandle._battleGroundName);
	            _asyncOperation.allowSceneActivation = false;
            }
            
            protected override void onTick(float dt)
            {
	            if(_asyncOperation == null) return;

	            if (Mathf.Approximately(0.9f, _asyncOperation.progress) && !_asyncOperation.isDone) {
		            _asyncOperation.allowSceneActivation = true;
		            return;
	            }
                
                if(!_asyncOperation.isDone) return;
                
                if (!_isSetupScene)
                    setupScene();

                Debug.Log($"[setupSceneTime] {Time.realtimeSinceStartup - _timeCounting }");
                BattleGroundHandle.switchState(EBattleStateType.Playing);
            }

            private void setupScene()
            {
                _isSetupScene = true;
                
                //第一时间把战场控制的Model放到Scene中
                BattleManager.BattleController.ModelController.EnterScene();

                //加载BattleLevelData
                BattleGroundHandle._levelData = Object.FindObjectOfType<BattleLevelData>();
                if (BattleGroundHandle == null)
                {
                    Debug.LogError("当前场景缺少BattleLevelData");
                    return;
                }
                
                //创建场景Actor
                foreach (var sceneActorModel in BattleGroundHandle._levelData.SceneActorModels) {
	                if (sceneActorModel.ActorType == EActorType.ActorRefreshPoint) {
		                var refreshPoint = (ActorRefreshPoint)sceneActorModel;
		                ActorManager.Instance.CreateActor(refreshPoint.CreateActorType, refreshPoint.ConfigId, (actor) => {
			                actor.SetAttr(ELogicAttr.AttrPosition, refreshPoint.transform.position, false);
			                actor.SetAttr(ELogicAttr.AttrRot, refreshPoint.transform.rotation, false);
		                });
		                continue;
	                }
	                ActorManager.Instance.CreateSceneActor(sceneActorModel, 0,
		                initPawnDefaultBirthPoint);
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

            protected override void onExit() {
                _asyncOperation = null;
            }
        }
    }
}