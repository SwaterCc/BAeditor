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
            private RoundStage _ready;

            /// <summary>
            /// 运行期
            /// </summary>
            private RoundStage _running;

            /// <summary>
            /// 结算期
            /// </summary>
            private RoundStage _ending;
            
            public Round()
            {
                
            }
            
            public void OnTick(float dt)
            {
                
            }
        }
    }
}