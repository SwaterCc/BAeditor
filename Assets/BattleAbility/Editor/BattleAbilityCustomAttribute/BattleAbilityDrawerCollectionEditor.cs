using System;

namespace BattleAbility.Editor.BattleAbilityCustomAttribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class BattleAbilityDrawerCollectionEditor : System.Attribute
    {
        public string GetInitValueFuncName;
       
        public BattleAbilityDrawerCollectionEditor(string funcName)
        {
            GetInitValueFuncName = funcName;
        }
    }
}