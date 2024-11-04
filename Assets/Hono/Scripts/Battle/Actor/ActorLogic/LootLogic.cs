#region

using System.Collections.Generic;
using Random = System.Random;

#endregion

namespace Hono.Scripts.Battle
{
	/// <summary>
	///     战利品
	/// </summary>
	public class LootLogic : ActorLogic
    {
        private LootSetting _lootSetting;
        private List<int> _randomSkill = new(16);
        private List<int> _randomLearnSkill = new(16);
        private static Random _random = new();
        public LootLogic(Actor actor) : base(actor) { }

        public void OnPawnPickUp(int actorUid)
        {
            if (!ActorManager.Instance.TryGetActor(actorUid, out var pawn))
            {
                ActorManager.Instance.RemoveActor(Uid);
                return;
            }

            //拾取特效
            if (pawn.Logic.TryGetComponent<VFXComp>(out var comp))
            {
                var setting = new VFXSetting()
                {
                    VFXBindType = EVFXType.InWorld,
                    Duration = 1,
                    Offset = new SVector3(0, 0.5f, 0),
                    VFXPath =
                        "Assets/BattleRes/VFX/3rd/Vefects/Anime VFX URP/Shared/Particles/PS_VFX_PickupCast.prefab"
                };

                comp.AddVFXObject(setting);
            }

            ActorManager.Instance.RemoveActor(Uid);

            BattleManager.CurBattle.LootController.CreateRougeCards(actorUid);
        }
    }
}