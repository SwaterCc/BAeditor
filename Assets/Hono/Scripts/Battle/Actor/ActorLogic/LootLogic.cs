#region

#endregion

namespace Hono.Scripts.Battle
{
    /// <summary>
    /// 战利品
    /// </summary>
    public class LootLogic : ActorLogic, IAPoolObject
    {
        private LootSetting _lootSetting;

        public void OnPawnPickUp(int actorUid)
        {
            /*
            //拾取特效
            if (!ActorManager.Instance.GetActor(actorUid, out var pawn)) {
                ActorManager.Instance.RemoveActor(Uid);
                return;
            }

            if (pawn.Logic.TryGetComponent<VFXComp>(out var comp)) {
                var setting = new VFXSetting() {
                    VFXBindType = EVFXType.InWorld,
                    Duration = 1,
                    Offset = new SVector3(0, 0.5f, 0),
                    VFXPath =
                        "Assets/BattleRes/VFX/3rd/Vefects/Anime VFX URP/Shared/Particles/PS_VFX_PickupCast.prefab"
                };

                comp.AddVFXObject(setting);
            }
            */

            ActorManager.Instance.RemoveActor(Uid);
            BattleManager.CurBattle.LootController.CreateRougeCards(actorUid);
        }

        public override void RecycleLogicObject()
        {
            AObjectPool<LootLogic>.Pool.Recycle(this);
        }
    }
}