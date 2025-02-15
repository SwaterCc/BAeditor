
using System.Collections.Generic;

namespace Hono.Scripts.Battle.Core
{
    public partial class ActorCtorConfig
    {
        public static readonly Dictionary<EUnitComponentKey, UnitComponentFactory> ComponentFactories = new()
        {
            { EUnitComponentKey.CombatComp, new UnitComponentFactory<CombatComp>() },
            { EUnitComponentKey.HateComp, new UnitComponentFactory<HateComp>() },
            { EUnitComponentKey.MoveComp, new UnitComponentFactory<MoveComp>() },
        };

        public static readonly Dictionary<EUnitComponentKey, IComponentParamParser> ParamParsers = new()
        {
            { EUnitComponentKey.CombatComp, new CombatCompParamParser() },
            { EUnitComponentKey.HateComp, new HateCompParamParser() },
            { EUnitComponentKey.ModelControllerComp, new ModelControllerCompParamParser() },
            { EUnitComponentKey.MoveComp, new MoveCompParamParser() },
        };
    }
}
