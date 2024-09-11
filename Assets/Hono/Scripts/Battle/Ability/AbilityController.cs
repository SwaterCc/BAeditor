using Hono.Scripts.Battle.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle {
	public partial class Actor {
		public class AbilityController {
			private readonly CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();
			private readonly Dictionary<int, Ability> _abilities = new();
			private readonly Dictionary<int, Dictionary<int,Ability>>_configDict = new();
			private readonly Actor _actor;

			public AbilityController(Actor actor) {
				_actor = actor;
			}

			public bool HasAbility(int configId) {
				foreach (var ability in _abilities) {
					if (ability.Value.ConfigId == configId)
						return true;
				}

				return false;
			}

			public bool TryGetAbility(int uid,out Ability ability)
			{
				return _abilities.TryGetValue(uid, out ability);
			}

			public Ability CreateAbility(int configId) {
				var ability = new Ability(_idGenerator.GenerateId(), _actor, configId);
				return ability;
			}

			public void AwardAbility(Ability ability, bool isRunNow) {
				if (_abilities.TryAdd(ability.Uid, ability)) {
					if (isRunNow) ability.Execute();
				}
			}

			/// <summary>
			/// 赋予能力
			/// </summary>
			/// <param name="actorId"></param>
			/// <param name="configId"></param>
			/// <param name="isRunNow"></param>
			public int AwardAbility(int configId, bool isRunNow) {
				var ability = new Ability(_idGenerator.GenerateId(), _actor, configId);
				if (_abilities.TryAdd(ability.Uid, ability)) {
					if (isRunNow) ability.Execute();
				}

				return ability.Uid;
			}

			/// <summary>
			/// 执行指定能力，会从资源检测开始，未Init的会主动调用一次Init
			/// </summary>
			public void ExecutingAbility(int uid) {
				if (_abilities.TryGetValue(uid, out var ability)) {
					ability.Execute();
				}
			}
			
			public void ExecutingAbilityByConfig(int configId) {
				foreach (var pair in _abilities)
				{
					if (pair.Value.ConfigId == configId)
					{
						pair.Value.Execute();
					}
				}
			}

			public void RemoveAbility(int uid) {
				if (_abilities.TryGetValue(uid, out var ability)) {
					ability.OnDestroy();
					_abilities.Remove(uid);
				}
			}

			public void Tick(float dt) {
				foreach (var abilityPair in _abilities) {
					var ability = abilityPair.Value;
					Ability.Context.UpdateContext((_actor, ability));
					ability.OnTick(dt);
					Ability.Context.ClearContext();
				}
			}
		}
	}
}