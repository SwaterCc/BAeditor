#region

using System;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public class SkillCdEventInfo : IEventInfo
    {
        public int SkillBelongActorUid;
        public int SkillId;
        public Action<Action<float>> AddCdTickFunc;
    }
}