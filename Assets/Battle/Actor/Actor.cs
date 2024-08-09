using System.Collections.Generic;

namespace Battle
{
    /// <summary>
    /// 游戏场景中对象的基类
    /// </summary>
    public class Actor : IVariableCollectionBind
    {
        /// <summary>
        /// Ability控制器
        /// </summary>
        public readonly AbilityController AbilityController;

        /// <summary>
        /// 状态机
        /// </summary>
        private StateMachine _stateMachine;

        /// <summary>
        /// 变量
        /// </summary>
        private readonly VariableCollection _variables;

        public Actor()
        {
            AbilityController = new AbilityController(this);
            _stateMachine = new StateMachine(this);
            _variables = new VariableCollection(8, this);
        }
        
        /// <summary>
        /// 测试用
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="ll"></param>
        /// <returns></returns>
        public int GetActorXXX(int i, float j, List<int> ll)
        {
            return 1;
        }

        public VariableCollection GetCollection()
        {
            return _variables;
        }
    }
}