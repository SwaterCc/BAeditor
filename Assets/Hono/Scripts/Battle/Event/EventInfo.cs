using System.Reflection;

namespace Hono.Scripts.Battle.Event {
	public interface IEventInfo { }

	public static class EventInfoExtension {
		public static void SetFieldsInAbilityVariables(this IEventInfo eventInfo, Ability ability) {
			foreach (var fieldInfo in eventInfo.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)) {
				var name = "EventInfo:" + fieldInfo.Name;
				ability.Variables.Set(name, fieldInfo.GetValue(eventInfo));
			}
		}

		public static void ClearFields(this IEventInfo eventInfo, Ability ability) {
			foreach (var fieldInfo in eventInfo.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)) {
				var name = "EventInfo:" + fieldInfo.Name;
				ability.Variables.Delete(name);
			}
		}
	}
}