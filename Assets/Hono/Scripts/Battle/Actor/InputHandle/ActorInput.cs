using UnityEngine;

namespace Hono.Scripts.Battle {
	public abstract class ActorInput {
		protected ActorLogic Logic { get; }

		private readonly ActorLogic.SkillComp _skillComp;
		protected ActorLogic.SkillComp SkillComp => _skillComp;

		protected bool NoSkillComp { get; }
		
		public Vector3 MoveInputValue { get; protected set; }

		protected ActorInput(ActorLogic logic) {
			Logic = logic;
			NoSkillComp = logic.TryGetComponent(out _skillComp);
		}
	}
	
	
	public class NoInput : ActorInput
	{
		public NoInput(ActorLogic logic) : base(logic) { }
	}
}