using System.Collections.Generic;

namespace Battle.Auto
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
                    else if(param1.IsAttribute)
                    {
                        var attr = Ability.Context.GetActor().GetAttr(param1.GetPropertyParam().PropertyType);
                        if (!attr.IsComposite && attr is SimpleAttribute<int> sAttr)
                        {
                            p1 = sAttr.Get();
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
                }
                else
                {
                    return 0;
                }
                
                return Ability.Context.GetActor().GetActorXXX(p1);
            }
            else
            {
                //报错
                return 0;
            }
        }
    }
}