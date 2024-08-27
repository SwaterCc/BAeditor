using Hono.Verse;
using System.Collections.Generic;

namespace Hono.Scripts.Battle
{
    public class DamageInfo
    {
        public int DamageConfigId;
        public List<int> HitTargets;
        public int SourceType;
        public int SourceId;
    }
}