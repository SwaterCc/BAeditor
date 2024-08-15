using System.Collections.Generic;
using Battle.Auto;
using Battle.Tools.CustomAttribute;
using UnityEngine;

namespace Battle.Tools
{
    #region ParamExtension

    public static class ParamExtension
    {
        /// <summary>
        /// 默认队列中只存在一个函数，直接运行队列
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="valueBox"></param>
        /// <returns></returns>
        public static bool TryCallFunc(this Queue<Param> queue, out IValueBox valueBox)
        {
            valueBox = null;
            var func = queue.Dequeue();
            if (func.IsFunc)
            {
                valueBox = queue.CallFunc(func);
                return true;
            }

            Debug.LogError("队首不是函数");
            return false;
        }

        /// <summary>
        /// 执行指定函数
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IValueBox CallFunc(this Queue<Param> queue, Param func)
        {
            var funcInfo = AbilityPreLoad.GetFuncInfo(func.FuncName);
            IValueBox[] funcParams = new IValueBox[funcInfo.ParamCount];

            for (int idx = 0; idx < funcInfo.ParamCount; idx++)
            {
                var param = queue.Dequeue();
                var paramType = funcInfo.ParamTypes[idx];
                if (param.IsBaseType)
                {
                    var getBaseFunc = AbilityPreLoad.GetFuncInfo("GetBaseValueBox", paramType.Name);
                    funcParams[idx] = (IValueBox)getBaseFunc.Invoke(null, param);
                }

                if (param.IsAttribute)
                {
                    var getAttrBox = AbilityPreLoad.GetFuncInfo("GetAttrBox");
                    funcParams[idx] = (IValueBox)getAttrBox.Invoke(Ability.Context.GetActor(), param);
                }

                if (param.IsVariable)
                {
                    var getVariableBox = AbilityPreLoad.GetFuncInfo("GetVariableBox", paramType.Name);
                    funcParams[idx] = (IValueBox)getVariableBox.Invoke(null, param);
                }

                if (param.IsFunc)
                {
                    funcParams[idx] = CallFunc(queue, param);
                }
            }

            //TODO:有消耗
            var res = (IValueBox)funcInfo.Invoke(null, funcParams);
            return res;
        }
    }

    #endregion

    #region AbilityExtension

    public static class AbilityExtension
    {
        [AbilityFuncTag(EFuncCacheFlag.OnlyCache)]
        public static void CreateHitBox(this Actor actor, int i)
        {
           
        }

        public static void AddAbility(int id)
        {
            
        }
        
        public static T GetAttr<T>(this Actor actor, EAttributeType attributeType)
        {
            return default;
        }

        [AbilityFuncTag(EFuncCacheFlag.OnlyCache)]
        public static IValueBox GetAttrBox(this Actor actor, Param param)
        {
            return Ability.Context.GetActor().GetAttrCollection().GetAttr(param.AttributeType).GetBox();
        }

        [AbilityFuncTag(EFuncCacheFlag.OnlyCache)]
        public static IValueBox GetBaseValueBox<T>(Param param)
        {
            if (typeof(T) == typeof(int))
            {
                return new ValueBox<int>(param.IntValue);
            }

            if (typeof(T) == typeof(long))
            {
                return new ValueBox<long>(param.LongValue);
            }

            if (typeof(T) == typeof(float))
            {
                return new ValueBox<float>(param.FloatValue);
            }

            if (typeof(T) == typeof(bool))
            {
                return new ValueBox<bool>(param.BoolValue);
            }

            return null;
        }

        public static VariableCollection GetVariableCollection(EVariableRange range)
        {
            switch (range)
            {
                case EVariableRange.Battleground:
                    return default;
                case EVariableRange.Actor:
                    return Ability.Context.GetActor().GetVariableCollection();
                case EVariableRange.Ability:
                    return Ability.Context.GetAbility().GetVariableCollection();
            }

            return null;
        }

        [AbilityFuncTag(EFuncCacheFlag.OnlyCache)]
        public static IValueBox GetVariableBox(EVariableRange range, string name)
        {
            VariableCollection collection = GetVariableCollection(range);

            if (collection != null)
            {
                return collection.GetVariable(name);
            }

            Debug.LogError("不应该走到这里");
            return default;
        }

        [AbilityFuncTag(EFuncCacheFlag.OnlyCache)]
        public static void CreateVariable(EVariableRange range, string name, IValueBox valueBox)
        {
            VariableCollection collection = GetVariableCollection(range);
            collection?.Add(name, valueBox);
        }

        public static Actor GetActor(int actorUId)
        {
            return null;
        }
    }

    #endregion
}