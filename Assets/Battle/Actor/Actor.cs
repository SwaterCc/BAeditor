using System.Collections.Generic;
using Battle.Auto;
using Battle.Tools;
using Battle.Tools.CustomAttribute;
using UnityEditor.Timeline;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// 游戏场景中对象的基类
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// Ability控制器
        /// </summary>
        public readonly AbilityController AbilityController;

        /// <summary>
        /// 状态机
        /// </summary>
        private StateMachine _stateMachine;

        private class ActorInterfaceImp : IVariableCollectionBind
        {
            private readonly Actor _actor;

            public ActorInterfaceImp(Actor actor)
            {
                _actor = actor;
            }

            public VariableCollection GetVariableCollection()
            {
                return _actor._variables;
            }
        }

        private readonly ActorInterfaceImp _actorImp;

        /// <summary>
        /// 变量
        /// </summary>
        private readonly VariableCollection _variables;

        private readonly AttrCollection _attrs;

        public Actor()
        {
            _actorImp = new ActorInterfaceImp(this);
            AbilityController = new AbilityController(this);
            _stateMachine = new StateMachine(this);
            _variables = new VariableCollection(8, _actorImp);
            _attrs = new AttrCollection();
        }

        public VariableCollection GetVariableCollection()
        {
            return _actorImp.GetVariableCollection();
        }

        public AttrCollection GetAttrCollection()
        {
            return _attrs;
        }
    }

    public static class ParamStackExtension
    {
        public static ICValueBox ParseParam(Param param)
        {
            ICValueBox res = null;
            
            if (param.IsBaseType)
            {
                var getBaseFunc = AbilityPreLoad.GetFuncInfo("GetBaseValueBox", paramType.Name);
                //TODO:有消耗
                res = (ICValueBox)getBaseFunc.Invoke(null, param);
            }

            if (param.IsAttribute)
            {
                var getAttrBox = AbilityPreLoad.GetFuncInfo("GetAttrBox");
                //TODO:有消耗
                res = (ICValueBox)funcInfo.Invoke(Ability.Context.GetActor(), param);
            }

            if (param.IsVariable)
            {
                var getVariableBox = AbilityPreLoad.GetFuncInfo("GetVariableBox", paramType.Name);
                //TODO:有消耗
                res = (ICValueBox)funcInfo.Invoke(null, param);
            }
            
        }
        
        public static ICValueBox CallFunc(this Stack<Param> stack, Param func)
        {
            var funcInfo = AbilityPreLoad.GetFuncInfo(func.FuncName);
            ICValueBox[] funcParams = new ICValueBox[funcInfo.ParamCount];

            for (int idx = 0; idx < funcInfo.ParamCount; idx++)
            {
                var param = stack.Pop();
                var paramType = funcInfo.ParamTypes[idx];
                if (param.IsBaseType)
                {
                    var getBaseFunc = AbilityPreLoad.GetFuncInfo("GetBaseValueBox", paramType.Name);
                    //TODO:有消耗
                    funcParams[idx] = (ICValueBox)getBaseFunc.Invoke(null, param);
                }

                if (param.IsAttribute)
                {
                    var getAttrBox = AbilityPreLoad.GetFuncInfo("GetAttrBox");
                    //TODO:有消耗
                    funcParams[idx] = (ICValueBox)funcInfo.Invoke(Ability.Context.GetActor(), param);
                }

                if (param.IsVariable)
                {
                    var getVariableBox = AbilityPreLoad.GetFuncInfo("GetVariableBox", paramType.Name);
                    //TODO:有消耗
                    funcParams[idx] = (ICValueBox)funcInfo.Invoke(null, param);
                }

                if (param.IsFunc)
                {
                    funcParams[idx] = CallFunc(stack,param);
                }
            }

            //TODO:有消耗
            var res = (ICValueBox)funcInfo.Invoke(null, funcParams);
            return res;
        }
    }
    
    public static class AbilityExtension
    {
        [AbilityFuncTag(EFuncCacheFlag.OnlyCache)]
        public static int GetActorXXX(this Actor actor, int i)
        {
            return 1;
        }

        public static T GetAttr<T>(this Actor actor, EAttributeType attributeType)
        {
            return default;
        }

        [AbilityFuncTag(EFuncCacheFlag.ShowVariableWindow | EFuncCacheFlag.ShowForeachWindow)]
        public static ICValueBox GetAttrBox(this Actor actor, Param param)
        {
            return Ability.Context.GetActor().GetAttrCollection().GetAttr(param.AttributeType).GetBox();
        }

        [AbilityFuncTag(EFuncCacheFlag.OnlyCache)]
        public static ICValueBox GetBaseValueBox<T>(Param param)
        {
            if (typeof(T) == typeof(int))
            {
                return new CValueBox<int>(param.IntValue);
            }

            if (typeof(T) == typeof(long))
            {
                return new CValueBox<long>(param.LongValue);
            }

            if (typeof(T) == typeof(float))
            {
                return new CValueBox<float>(param.FloatValue);
            }

            if (typeof(T) == typeof(bool))
            {
                return new CValueBox<bool>(param.BoolValue);
            }

            return null;
        }

        public static ICValueBox GetVariableBox(EVariableRange range, string name)
        {
            VariableCollection collection = null;
            switch (range)
            {
                case EVariableRange.Battleground:
                    return default;
                case EVariableRange.Actor:
                    collection = Ability.Context.GetActor().GetVariableCollection();
                    break;
                case EVariableRange.Ability:
                    collection = Ability.Context.GetAbility().GetVariableCollection();
                    break;
            }

            if (collection != null)
            {
                return collection.GetVariable(name);
            }

            Debug.LogError("不应该走到这里");
            return default;
        }


        public static Actor GetActor(int actorUId)
        {
            return null;
        }
    }
}