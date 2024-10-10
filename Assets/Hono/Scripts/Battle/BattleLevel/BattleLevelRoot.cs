using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Hono.Scripts.Battle.Scene;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 关卡流程
    /// </summary>
    public partial class BattleLevelRoot
    {
        private BattleLevelData _battleLevelData;

        private BattleLevelControl _battleLevelControl;

        public bool AllLoadedFinish { get; private set; }

        private PawnGroupInfos _pawnGroupInfos;
        
        public async void Setup(BattleLevelData battleLevelData, bool useSave)
        {
            _battleLevelData = battleLevelData;
            _pawnGroupInfos = null;

            List<UniTask> tasks = new List<UniTask>();
            tasks.Add(buildScene());
            if (!useSave)
            {
                tasks.Add(buildPawnGroup());
            }
            await UniTask.WhenAll(tasks);

            AllLoadedFinish = true;

            EnterBattleLevel();
        }

        private async UniTask buildScene()
        {
            //创建静态Actor（玩家位置，刷怪点，触发器）

            //创建BattleLevelControl
        }

        private async UniTask buildPawnGroup()
        {
            //等待队伍编队返回
            await UniTask.WaitUntil(() => _pawnGroupInfos != null);

            //创建玩家Actor
        }

        /// <summary>
        /// 数据准备完毕后开始游戏
        /// </summary>
        public void EnterBattleLevel()
        {
            
        }

        public void OnTick(float dt) { }

        public void OnUpdate(float dt) { }

        public void ExitBattleLevel() { }
    }
}