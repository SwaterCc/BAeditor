using System;
using UnityEngine;

namespace Battle
{
    public static class VariableHelper
    {
        public static bool TryGetVariable<T>(this IVariableCollectionBind bind, string name, out Variable<T> variable)
        {
            return bind.GetVariableCollection().TryGetVariable(name, out variable);
        }

        public static Variable<T> GetVariable<T>(this IVariableCollectionBind bind, string name)
        {
            return bind.GetVariableCollection().GetVariable<T>(name);
        }

        public static T GetVariableValue<T>(EVariableRange range, string name)
        {
            switch (range)
            {
                case EVariableRange.Battleground:
                    return default;
                case EVariableRange.Actor:
                    return Ability.Context.GetActor().GetVariable<T>(name).Get();
                case EVariableRange.Ability:
                    return Ability.Context.GetAbility().GetVariable<T>(name).Get();
            }

            Debug.LogError("不应该走到这里");
            return default;
        }


        public static void InsertVariable(this VariableCollection collection, EVariableType type, string name,
            object value)
        {
            Variable variable = null;
            switch (type)
            {
                case EVariableType.Int:
                    variable = new Variable<int>(name, (int)value);
                    break;
                case EVariableType.Long:
                    variable = new Variable<long>(name, (long)value);
                    break;
                case EVariableType.Bool:
                    variable = new Variable<bool>(name, (bool)value);
                    break;
                case EVariableType.Float:
                    variable = new Variable<float>(name, (float)value);
                    break;
                case EVariableType.String:
                    variable = new Variable<string>(name, (string)value);
                    break;
            }

            collection.Add(name, variable);
        }
    }
}