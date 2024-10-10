namespace Hono.Scripts.Battle
{
    public partial class BattleLevelRoot
    {
        /// <summary>
        /// 战斗轮次
        /// </summary>
        public class Round
        {
            /// <summary>
            /// 准备期
            /// </summary>
            private RoundStage _ready = new();

            /// <summary>
            /// 运行期
            /// </summary>
            private RoundStage _running = new();

            /// <summary>
            /// 结算期
            /// </summary>
            private RoundStage _ending = new();

            /// <summary>
            /// 当前阶段
            /// </summary>
            private RoundStage _curStage;

            public void BeginNewRound()
            {
                
            }
            
            public void OnTick(float dt)
            {
                if(_curStage == null) return;

                _curStage.Tick(dt);

                if (_curStage.CanExit)
                {
                    _curStage.Exit();
                    _curStage = _curStage.Next;
                    _curStage.Enter();
                }
            }
        }
    }
}