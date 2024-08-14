namespace Battle
{
    /// <summary>
    /// 玩家和怪物的基础状态机基类
    /// 目前定义的状态：闲置，移动，战斗，硬直，重伤（不是死亡）
    /// </summary>
    public class StateMachine : ITick
    {
        private Actor _actor;

        public StateMachine(Actor actor)
        {
            _actor = actor;
        }

        public void Tick(float dt)
        {
            
        }
    }
}