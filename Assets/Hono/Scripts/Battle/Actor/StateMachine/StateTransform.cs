#region

using System;

#endregion

namespace Hono.Scripts.Battle {
	
		public class StateTransform {
			public EActorStateType SwitchTo { get; }
			private readonly Func<bool> _transCondition;

			public StateTransform(EActorStateType switchTo) {
				SwitchTo = switchTo;
			}

			public bool CanSwitch() {
				return false;
			}
		}
	
}