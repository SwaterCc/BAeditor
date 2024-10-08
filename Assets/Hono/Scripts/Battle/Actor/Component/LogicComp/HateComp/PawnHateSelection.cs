namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        /// <summary>
        /// 玩家仇恨简单处理，默认找距离自己最近的且不超过最大离队距离的敌对单位
        /// </summary>
        public class PawnHateSelection : HateSelection
        {
            public override int GetHateTargetUid()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}