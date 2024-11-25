namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {
        public class Buff : IAPoolObject
        {
            public ActorLogic Logic { get; private set; }
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

            public void OnRent(in ActorLogic logic, in int sourceActorUid, in BuffData buffData)
            {
                Logic = logic;
                SourceActorUid = sourceActorUid;
                BuffData = buffData;
                ConfigId = buffData.id;
                LayerCount = buffData.InitLayer;
                Ability = Logic.Self.Abilities.AwardAbility(BuffData.id);
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
                Logic.Self.Abilities.RemoveAbility(Ability.Id);
                Ability = null;
                Logic = null;
                SourceActorUid = 0;
                BuffData = null;
                LayerCount = 0;
                ConfigId = 0;
            }
        }
    }
}