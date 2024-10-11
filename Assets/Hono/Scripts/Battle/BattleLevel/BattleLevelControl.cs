using Hono.Scripts.Battle.Scene;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    //当前关卡控制
    public partial class BattleLevelController : ActorLogic
    {
        private VFXWorldComp _vfxWorldComp;

        private int _currentRoundIndex;

        private readonly BattleLevelData _levelData;

        private readonly Round _round;

        public BattleLevelController(Actor actor, BattleLevelData levelData) : base(actor)
        {
            _levelData = levelData;
            _round = new Round(this);
        }

        protected override void setupComponents()
        {
            _vfxWorldComp = new VFXWorldComp(this);
            addComponent(_vfxWorldComp);
        }

        public void AddWorldVFX(VFXObject obj)
        {
            _vfxWorldComp.AddVFXObjectToWorld(obj);
        }
        
        public void GameReadyFinish()
        {
            _currentRoundIndex = 0;

            if (_currentRoundIndex >= _levelData.RoundDatas.Count)
            {
                Debug.LogError("关卡数据至少要有一回合！");
                return;
            }
            
            //开始游戏的第一回合
            _round.BeginNewRound(_levelData.RoundDatas[_currentRoundIndex]);
        }

        protected override void onTick(float dt)
        {
            _round.OnTick(dt);
        }

        private void onRoundFailed()
        {
            if (_levelData.CanRepeat)
            {
                //重新开始
                _round.BeginNewRound(_levelData.RoundDatas[_currentRoundIndex]);
                return;
            }
            //结算战斗
        }

        private void onRoundFinish()
        {
            ++_currentRoundIndex;
            if (_currentRoundIndex >= _levelData.RoundDatas.Count)
            {
                //通关了，打开通关UI
                return;
            }
            //下一回合
            _round.BeginNewRound(_levelData.RoundDatas[_currentRoundIndex]);
        }
    }
}