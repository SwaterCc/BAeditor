#region

using System.Reflection;

#endregion

namespace Hono.Scripts.Battle.Event
{
    public interface IEventInfo
    {
        public void Clear();
    }

    public static class EventInfoExtension
    {
        public static void SetFieldsInAbilityVariables(this IEventInfo eventInfo, Ability ability)
        {
            foreach (var fieldInfo in eventInfo.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var name = "EventInfo:" + fieldInfo.Name;
                ability.Vairables.Set(name, fieldInfo.GetValue(eventInfo));
            }
        }

        public static void ClearFields(this IEventInfo eventInfo, Ability ability)
        {
            foreach (var fieldInfo in eventInfo.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var name = "EventInfo:" + fieldInfo.Name;
                ability.Vairables.Delete(name);
            }
        }
    }
}