using System;
using System.Collections.Generic;

namespace Battle.Auto
{
    public static class WrapFuncMain
    {
        public static void InvokeFunc(int funcId, ParamCollection collection, out int res)
        {
            res = default;
            switch (funcId)
            {
                case 1:
                    res = FuncWrapTemplate._GetActorXXX_Wrap_(collection);
                    break;
            }
        }

        public static void InvokeFunc(int funcId, ParamCollection collection, out float res)
        {
            res = default;
            switch (funcId)
            {
                case 2:
                    res = FuncWrapTemplate._GetActorXXX_Wrap_(collection);
                    break;
            }
        }
    }
}