using System.Collections.Generic;
using Battle.Tools;

namespace Battle
{
    public class AbilityController
    {
        private readonly Dictionary<int, Ability> _abilities = new();

        private readonly CommonUtility.IdGenerator _idGenerator = CommonUtility.GetIdGenerator();

        /// <summary>
        /// 赋予能力
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="isRunNow"></param>
        public void AwardAbility(int configId, bool isRunNow)
        {
            var ability = new Ability(_idGenerator.GenerateId(), configId);
            if (ability.GetCheckerRes(EAbilityCycleType.OnPreAwardCheck)
                && _abilities.TryAdd(ability.Uid, ability))
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