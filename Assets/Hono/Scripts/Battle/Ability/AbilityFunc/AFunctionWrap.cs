using Hono.Scripts.Battle.Base;

namespace Hono.Scripts.Battle
{
    public partial class Ability
    {
        public abstract class AFunctionWrap
        {
            public int ParamCount { get; protected set; }

            public bool Invoke(Ability caller, AParamsParser.AFuncParams @params, out object value)
            {
                ARunningTime.UpdateContext(caller);
                var result = OnInvoke(caller, @params, out value);
                ARunningTime.UpdateContext(null);
                return result;
            }

            protected abstract bool OnInvoke(Ability caller, AParamsParser.AFuncParams @param, out object value);
        }
        //@Auto
        public class AFuncWrap_GetBuffLayer : AFunctionWrap
        {
            public AFuncWrap_GetBuffLayer()
            {
                ParamCount = 2;
            }

            protected override bool OnInvoke(Ability caller, AParamsParser.AFuncParams @params, out object value)
            {
                value = null;
                if (@params.Count <= ParamCount)
                {
                    return false;
                }

                RefInt param0 = @params.Pop<RefInt>();

                RefInt param1 = @params.Pop<RefInt>();

                var result = AObjectPool<RefInt>.Pool.Rent();

                result.Value = AbilityFunctionDefine.GetBuffLayer(param0, param1);
                value = result;

                return true;
            }
        }
    }
}