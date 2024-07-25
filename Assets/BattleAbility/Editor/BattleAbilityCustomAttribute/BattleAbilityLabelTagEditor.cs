using System;

namespace BattleAbility.Editor.BattleAbilityCustomAttribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class BattleAbilityLabelTagEditor : System.Attribute
    {
        public enum ELabeType
        {
            None,
            Int32,
            Long,
            Float,
            String,
            Enum,
            List,
            Dict,
            Dict2ListOnlyKey,
            Dict2ListOnlyValue,
        }

        public string LabelText;
        public ELabeType LabeType;

        public BattleAbilityLabelTagEditor(string labelText, ELabeType labeType = ELabeType.String)
        {
            LabelText = labelText;
            LabeType = labeType;
        }
    }
}