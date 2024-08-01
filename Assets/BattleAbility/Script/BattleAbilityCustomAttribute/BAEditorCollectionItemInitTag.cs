using System;

namespace BattleAbility.Editor.BattleAbilityCustomAttribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class BAEditorCollectionItemInitTag : System.Attribute
    {
        public string GetInitValueFuncName;
       
        public BAEditorCollectionItemInitTag(string funcName)
        {
            GetInitValueFuncName = funcName;
        }
    }
}