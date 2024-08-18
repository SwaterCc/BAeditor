using System.Collections.Generic;
using Battle.Auto;
using Battle.Tools.CustomAttribute;
using UnityEngine;

namespace Battle.Tools
{
    #region AbilityExtension

    public static class AbilityExtension
    {
        [AbilityFuncTag(EFuncCacheFlag.OnlyCache)]
        public static void CreateHitBox(this Actor actor, int hitData)
        {
            var hitBox = new HitBox(hitData);
            BattleManager.Instance.Add(hitBox);
        }

        public static void AddAbility(int id)
        {
            
        }
        
        public static T GetAttr<T>(this Actor actor, EAttributeType attributeType)
        {
            return default;
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