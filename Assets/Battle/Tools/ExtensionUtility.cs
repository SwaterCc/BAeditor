using System.Collections.Generic;
using Battle.Auto;
using Battle.Tools.CustomAttribute;
using UnityEngine;

namespace Battle.Tools
{
    #region AbilityCacheFuncDefine

    public static class AbilityCacheFuncDefine
    {
        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void CreateHitBox(this Actor actor, int hitDataId)
        {
            var hitBox = new HitBox(hitDataId);
            BattleManager.Instance.Add(hitBox);
        }

        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void AddAbility(int actorUid, int ability, bool isRunNow)
        {
            var actor = BattleManager.Instance.GetActor(actorUid);
            if (actor != null)
            {
                var ab = new Ability(ability);
                actor.GetAbilityController().AwardActorAbility(ab, false);
            }
        }

        [AbilityFuncCache(EFuncCacheFlag.Variable|EFuncCacheFlag.Branch)]
        public static object GetAttrBox(EAttributeType attributeType)
        {
            var attr = Ability.Context.BelongActor.GetAttrCollection().GetAttr(attributeType);
            return attr.GetBox();
        }

        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void SetAttrSimple(EAttributeType attributeType, object value)
        {
            var attr = Ability.Context.BelongActor.GetAttrCollection().GetAttr(attributeType);
            if (!attr.IsComposite)
            {
                ((SimpleAttribute)attr).OnlySet(value);
            }
        }
        [AbilityFuncCache(EFuncCacheFlag.Action)]
        public static void SetAttrElement(EAttributeType attributeType, EAttrElementType elementType, object value)
        {
            var attr = Ability.Context.BelongActor.GetAttrCollection().GetAttr(attributeType);
            if (attr.IsComposite)
            {
                ((CompositeAttribute)attr).SetElementAttr(elementType, (float)value);
            }
        }

        public static VariableCollection GetVariableCollection(EVariableRange range)
        {
            switch (range)
            {
                case EVariableRange.Battleground:
                    return default;
                case EVariableRange.Actor:
                    return Ability.Context.BelongActor.GetVariableCollection();
                case EVariableRange.Ability:
                    return Ability.Context.CurrentAbility.GetVariableCollection();
            }

            return null;
        }

        [AbilityFuncCache(EFuncCacheFlag.Action|EFuncCacheFlag.Branch|EFuncCacheFlag.Variable)]
        public static object GetVariable(EVariableRange range, string name)
        {
            VariableCollection collection = GetVariableCollection(range);

            if (collection != null)
            {
                return collection.GetVariable(name);
            }

            Debug.LogError("不应该走到这里");
            return default;
        }

        [AbilityFuncCache(EFuncCacheFlag.OnlyCache)]
        public static void ChangeVariable(EVariableRange range, string name, object valueBox)
        {
            VariableCollection collection = GetVariableCollection(range);
            collection.ChangeValue(name, valueBox);
        }

        [AbilityFuncCache(EFuncCacheFlag.OnlyCache)]
        public static void CreateVariable(EVariableRange range, string name, object valueBox)
        {
            VariableCollection collection = GetVariableCollection(range);
            collection?.Add(name, valueBox);
        }

        [AbilityFuncCache(EFuncCacheFlag.Variable | EFuncCacheFlag.Branch)]
        public static int GetActorSelf()
        {
            return Ability.Context.BelongActor.Uid;
        }

        #region 数学函数

        [AbilityFuncCache(EFuncCacheFlag.Variable)]public static object Add(float a, float b) => a + b;
        [AbilityFuncCache(EFuncCacheFlag.Variable)]public static int Add(int a, int b) => a + b;
       
        [AbilityFuncCache(EFuncCacheFlag.Variable)]public static float Subtract(float a, float b) => a - b;
        [AbilityFuncCache(EFuncCacheFlag.Variable)]public static int Subtract(int a, int b) => a - b;
        
        [AbilityFuncCache(EFuncCacheFlag.Variable)]public static float Multiply(float a, float b) => a * b;
        [AbilityFuncCache(EFuncCacheFlag.Variable)]public static int Multiply (int a, int b) => a * b;
        
        [AbilityFuncCache(EFuncCacheFlag.Variable)]public static float Divide(float a, float b) => a / b;
        [AbilityFuncCache(EFuncCacheFlag.Variable)]public static int Divide(int a, int b) => a / b;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]public static int SelfAdditive(int a) => ++a;
        [AbilityFuncCache(EFuncCacheFlag.Variable)]public static float SelfAdditive(float a) => ++a;

        [AbilityFuncCache(EFuncCacheFlag.Variable)]public static int SelfSubtracting(int a) => --a;
        [AbilityFuncCache(EFuncCacheFlag.Variable)]public static float SelfSubtracting(float a) => --a;
        #endregion

    }

    #endregion
}