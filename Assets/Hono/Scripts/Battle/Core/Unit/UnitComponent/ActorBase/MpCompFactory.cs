using Hono.Scripts.Battle.Core;

namespace Hono.Scripts.Battle.Core
{
    public class MpCompFactory : IUnitComponentFactory
    {
        public UnitComponent CreateComponent()
        {
            return new MpComp();
        }
    }
}