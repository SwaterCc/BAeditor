using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Event;
using Hono.Scripts.Battle.Scene;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 关卡流程
    /// </summary>
    public class BattleLevelRoot
    {
        private BattleLevelData _battleLevelData;

        private BattleLevelController _battleLevelControl;

        private PawnGroupInfos _pawnGroupInfos;

        public async void Setup(BattleLevelData battleLevelData, bool useSave)
        {
            _battleLevelData = battleLevelData;
            _pawnGroupInfos = null;

            //打开loading
            BattleEventManager.Instance.TriggerGlobalEvent(EBattleEventType.CallLoadingUI,
                new CallUIEventInfo() { UIFlag = true });
            
            buildScene();
            
            await buildPawnGroup();
            
            BattleEventManager.Instance.TriggerGlobalEvent(EBattleEventType.CallLoadingUI,
                new CallUIEventInfo() { UIFlag = false });
            
            _battleLevelControl.GameReadyFinish();
        }

        private void buildScene()
        {
            //创建静态Actor（玩家位置，刷怪点，触发器）
            ActorManager.Instance.CreateStaticActor();

            //创建BattleLevelControl
            _battleLevelControl = ActorManager.Instance.GetBattleControl(_battleLevelData);
        }

        private async UniTask buildPawnGroup()
        {
            if (_pawnGroupInfos == null)
            {
                //打开编队界面
                BattleEventManager.Instance.TriggerGlobalEvent(EBattleEventType.CallPawnGroupEditUI,
                    new CallUIEventInfo() { UIFlag = true });
            }
            
            //等待队伍编队返回
            await UniTask.WaitUntil(() => _pawnGroupInfos != null);
            
            //关闭编队界面
            BattleEventManager.Instance.TriggerGlobalEvent(EBattleEventType.CallPawnGroupEditUI,
                new CallUIEventInfo() { UIFlag = false });

            //创建玩家Actor
        }

        public void OnTick(float dt)
        {
            ActorManager.Instance.Tick(dt);
        }

        public void OnUpdate(float dt)
        {
            ActorManager.Instance.Update(dt);
        }

        public void UnInstall()
        {
            ActorManager.Instance.ClearAllActor();
        }
    }
}