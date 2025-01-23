using Hono.Scripts.Battle.AbilitySystem;
using Hono.Scripts.Battle.Core;

namespace Hono.Scripts.Battle
{
    public class Buff : IAPoolObject
    {
        public Unit Logic { get; private set; }
        public BuffData BuffData { get; private set; }

        /// <summary>
        /// Buff来源ActorUid
        /// </summary>
        public int SourceActorUid { get; private set; }

        /// <summary>
        /// Buff ConfigID
        /// </summary>
        public int ConfigId { get; private set; }

        /// <summary>
        /// Buff Layer 受上限约束
        /// </summary>
        public int LayerCount { get; private set; }

        /// <summary>
        /// Buff内核Ability
        /// </summary>
        public Ability Ability { get; private set; }

        public void OnRent(Unit unit, in int sourceActorUid, in BuffData buffData)
        {
            Logic = unit;
            SourceActorUid = sourceActorUid;
            BuffData = buffData;
            ConfigId = buffData.id;
            LayerCount = buffData.InitLayer;
            Ability = Logic.AddAbility(BuffData.id);
            Ability.Execute();
        }

        public void AddLayer(int layerCount)
        {
            LayerCount += layerCount;
            Ability.Stop();
            Ability.Execute();
        }

        public void OnRecycle()
        {
            Logic.RemoveAbility(Ability.Id);
            Ability = null;
            Logic = null;
            SourceActorUid = 0;
            BuffData = null;
            LayerCount = 0;
            ConfigId = 0;
        }
    }
}