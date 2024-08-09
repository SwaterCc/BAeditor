using System.Collections.Generic;

namespace Battle.Auto.WrapFunc
{
    public static class FuncWrapTemplate
    {
        public static int _GetActorXXX_Wrap_(ParamCollection collection)
        {
            if (Ability.Context == null || Ability.Context.IsNotRunning)
            {
                //报错
                return 0;
            }

            if (collection.Length == 3)
            {
                int p1;
                float p2;
                List<int> p3;
                if (collection.TryGetParam(0, out var param1))
                {
                    if (param1.NotCustom && !param1.TryGetInt(out p1))
                    {
                        return 0;
                    }

                    if (param1.IsVariable)
                    {
                        p1 = VariableHelper.GetVariableValue<int>(param1.GetVariableParam().Range, param1.GetVariableParam().Name);
                    }
                    else if(param1.IsProperty)
                    {
                        
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
                if (!(collection.TryGetParam(0, out var param2) || param2.TryGetFloat(out  p2)))
                {
                    return 0;
                }
                if (!(collection.TryGetParam(0, out var param2) || param2.TryGetFloat(out  p3)))
                {
                    return 0;
                }


                return Ability.Context.GetActor().GetActorXXX(p1, p2, p3);
            }
            else
            {
                //报错
                return 0;
            }
        }
    }
}