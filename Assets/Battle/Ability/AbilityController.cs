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
                
            }
        }

        /// <summary>
        /// 给予能力静态版本
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        /// <param name="isRunNow"></param>
        public static void AwardAbility(Actor actor, Ability ability, bool isRunNow = false)
        {
            actor.AbilityController.AwardActorAbility(ability, isRunNow);
        }

        /// <summary>
        /// 执行指定能力，会从资源检测开始，未Init的会主动调用一次Init
        /// </summary>
        public void ExecutingAbility(int id)
        {
            
        }

        public void Tick(float dt)
        {
            foreach (var abilityPair in _abilities)
            {
                var ability = abilityPair.Value;
                ability.OnTick(dt);
                Ability.Context.ClearContext();
            }
        }
    }
}