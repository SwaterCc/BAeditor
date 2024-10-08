namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        /// <summary>
        /// 怪物仇恨简单处理，优先仇恨离自己最近的玩家角色
        /// </summary>
        public class MonsterHateSelection : HateSelection
        {
            public override int GetHateTargetUid()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}