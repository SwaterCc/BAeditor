using System.Collections.Generic;

namespace Battle.Auto
{
    public static class FuncWrapTemplate
    {
        //暂时保留，用反射仍然存在很明显的类型转换问题，后续再考虑优化吧
        
        /*
        public static int _GetActorXXX_Wrap_(ParamStack stack)
        {
            if (Ability.Context == null || Ability.Context.IsNotRunning)
            {
                //报错
                return 0;
            }

            if (stack.Length == 1)
            {
                int p1;
                if (stack.TryGetParam(0, out var param1))
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
                        var attr = Ability.Context.GetActor().GetAttrBox(param1.GetAttributeParam().AttributeType);
                        p1 = ((ValueBox<int>)attr.Get()).Get();
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
        */
    }
}