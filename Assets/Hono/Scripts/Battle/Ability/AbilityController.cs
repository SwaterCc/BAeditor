using Hono.Scripts.Battle.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    public class AbilityController
    {
        private readonly Dictionary<int, Ability> _abilities = new();

        private readonly CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();

        private ActorLogic _logic;
        public AbilityController(ActorLogic logic)
        {
            _logic = logic;
        }
        
        public bool HasAbility(int configId) {
	        foreach (var ability in _abilities) {
		        if (ability.Value.ConfigId == configId)
			        return true;
	        }

	        return false;
        }

        public void Show() {
	        foreach (var ability in _abilities) {
		       Debug.Log($"ability UID{ability.Key} ,ConfigId {ability.Value.ConfigId}");
	        }
        }

        public Ability CreateAbility(int configId)
        {
            var ability = new Ability(_idGenerator.GenerateId(), _logic.Uid, configId);
            return ability;
        }

        public void AwardAbility(Ability ability, bool isRunNow)
        {
            if (_abilities.TryAdd(ability.Uid, ability))
            {
                if (isRunNow) ability.Execute();
            }
        }
        
        /// <summary>
        /// 赋予能力
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="configId"></param>
        /// <param name="isRunNow"></param>
        public void AwardAbility(int configId, bool isRunNow)
        {
            var ability = new Ability(_idGenerator.GenerateId(),_logic.Uid , configId);
            if (_abilities.TryAdd(ability.Uid, ability))
            {
                if (isRunNow) ability.Execute();
            }
        }

        /// <summary>
        /// 执行指定能力，会从资源检测开始，未Init的会主动调用一次Init
        /// </summary>
        public void ExecutingAbility(int uid)
        {
            if (_abilities.TryGetValue(uid, out var ability))
            {
                ability.Execute();
            }
        }

        public void Tick(ActorLogic actorLogic, float dt)
        {
            foreach (var abilityPair in _abilities)
            {
                var ability = abilityPair.Value;
                Ability.Context.UpdateContext((actorLogic, ability));
                ability.OnTick(dt);
                Ability.Context.ClearContext();
            }
        }
    }
}