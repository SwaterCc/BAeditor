using Battle.Ability;

namespace Battle
{
    /// <summary>
    /// 游戏场景中对象的基类
    /// </summary>
    public class Actor
    {
        public readonly AbilityController AbilityController;

        public Actor()
        {
            AbilityController = new AbilityController(this);
        }
    }
}