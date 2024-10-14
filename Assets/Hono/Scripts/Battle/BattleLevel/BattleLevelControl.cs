using Hono.Scripts.Battle.Scene;
using UnityEngine;

namespace Hono.Scripts.Battle
{
    //当前关卡控制
    //Actor数据统计波次挂钩
    //当前波次总共有多少Actor，用阵营索引(刷怪器的怪物是一开始就全部创建了)
    //当前波次当前还有多少Actor，用阵营索引(怪物死亡时会记录)
    //玩家阵营击杀了多少Actor，用阵营索引(behit组件中判定死亡时需要记录是否为击杀，击杀者uid，击杀者阵营)
    public partial class BattleController : ActorLogic
    {
        private VFXWorldComp _vfxWorldComp;

        public BattleControllerModel ModelController => (BattleControllerModel)Actor.ModelController;
        
        public BattleController(Actor actor) : base(actor) { }

        protected override void setupComponents()
        {
            _vfxWorldComp = new VFXWorldComp(this);
            addComponent(_vfxWorldComp);
        }

        public void AddWorldVFX(VFXObject obj)
        {
            _vfxWorldComp.AddVFXObjectToWorld(obj);
        }

        public void RunAbility(int abilityConfigId)
        {
            AbilityController.AwardAbility(abilityConfigId, true);
        }

        public void RemoveAbility(int abilityConfigId)
        {
            AbilityController.RemoveAbility(abilityConfigId);
        }
    }
}