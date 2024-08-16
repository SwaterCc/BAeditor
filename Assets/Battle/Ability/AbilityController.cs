using System.Collections.Generic;

namespace Battle
{
    public class AbilityController : ITick
    {
        private readonly Dictionary<int, Ability> _abilities = new();

        private Actor _actor;

        public AbilityController(Actor actor)
        {
            _actor = actor;
        }

        /// <summary>
        /// 赋予能力
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="isRunNow"></param>
        public void AwardActorAbility(Ability ability, bool isRunNow)
        {
            if (ability.GetCheckerRes(EAbilityCycleType.OnPreAwardCheck)
                && _abilities.ContainsKey(ability.Uid))
            {
                _abilities.Add(ability.Uid, ability);
                if (isRunNow) ability.Execute();
            }
        }

        /// <summary>
        /// 执行指定能力，会从资源检测开始，未Init的会主动调用一次Init
        /// </summary>
        public void ExecutingAbility(int id)
        {
            if (_abilities.TryGetValue(id, out var ability))
            {
                ability.Execute();
            }
        }

        public void Tick(float dt)
        {
            foreach (var abilityPair in _abilities)
            {
                var ability = abilityPair.Value;
                Ability.Context.UpdateContext((_actor, ability));
                ability.OnTick(dt);
                Ability.Context.ClearContext();
            }
        }
    }
}