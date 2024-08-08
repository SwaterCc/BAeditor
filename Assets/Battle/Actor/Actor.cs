using Battle.Ability;

namespace Battle
{
    /// <summary>
    /// 游戏场景中对象的基类
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// Ability控制器
        /// </summary>
        public readonly AbilityController AbilityController;
        
        /// <summary>
        /// 状态机
        /// </summary>
        private StateMachine _stateMachine;
        
        
        public Actor()
        {
            AbilityController = new AbilityController(this);
            _stateMachine = new StateMachine(this);
        }
    }
}