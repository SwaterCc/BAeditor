using Hono.Scripts.Battle.Core;

namespace Hono.Scripts.Battle.AbilityFramework
{
    /// <summary>
    ///     编辑器默认显示函数
    ///     不允许重载函数
    ///     尽量不要在get函数里直接获取对象
    ///     不要随便改函数名
    /// </summary>
    public partial class AFunctionDefine
    {
        /// <summary>
        /// 当前运行的Ability
        /// </summary>
        private Ability AContext { get; }

        /// <summary>
        /// Ability所属的Actor
        /// </summary>
        private Unit Actor => AContext.Unit;

        internal AFunctionDefine(Ability ability)
        {
            AContext = ability;
        }

        /// <summary>
        /// 获取Actor，当actorUid小于等于0时返回调用者Actor
        /// </summary>
        /// <param name="actorUid"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        private bool tryGetActor(int actorUid, out Actor actor)
        {
            actor = null;
            if (actorUid <= 0)
            {
                actor = AContext.Unit as Actor;
            }
            else
            {
                actor = World.Query.GetUnit(actorUid) as Actor;
            }

            return actor != null;
        }
    }
}