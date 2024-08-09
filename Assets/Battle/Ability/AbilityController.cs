using System.Collections.Generic;

namespace Battle
{
    public class AbilityController
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
        public void GiveActorAbility(Ability ability, bool isRunNow)
        {
            if (ability.PreGiveCheck() && _abilities.ContainsKey(ability.Uid))
            {
                _abilities.Add(ability.Uid, ability);
                if (isRunNow)
                {
                    ability.Init();
                    ability.State = EAbilityState.AbilityReady;
                }
            }
        }

        /// <summary>
        /// 给予能力静态版本
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        /// <param name="isRunNow"></param>
        public static void GiveAbility(Actor actor, Ability ability, bool isRunNow = false)
        {
            actor.AbilityController.GiveActorAbility(ability, isRunNow);
        }

        /// <summary>
        /// 执行指定能力，会从资源检测开始，未Init的会主动调用一次Init
        /// </summary>
        public void ExecutAbility(int id)
        {
            if (_abilities.TryGetValue(id, out var ability))
            {
                if (ability.State == EAbilityState.UnInit)
                {
                    ability.Init();
                }

                //下一帧会开始执行
                ability.State = EAbilityState.AbilityReady;
            }
        }

        public void Tick(float dt)
        {
            foreach (var abilityPair in _abilities)
            {
                var ability = abilityPair.Value;
                if (ability.State == EAbilityState.UnInit)
                {
                    ability.Init();
                }

                if (ability.State == EAbilityState.AbilityReady)
                {
                    if (ability.CheckCondition())
                    {
                        Ability.Context.UpdateContext((_actor, ability));
                        ability.PreExecute();
                    }
                }

                if (ability.State == EAbilityState.Executing)
                {
                    ability.Executing();
                }

                if (ability.State == EAbilityState.EndExecute)
                {
                    ability.EndExecute();
                    Ability.Context.ClearContext();
                }
            }
        }
    }
}