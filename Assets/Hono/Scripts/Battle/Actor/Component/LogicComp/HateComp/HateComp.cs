namespace Hono.Scripts.Battle
{
    public partial class ActorLogic
    {

        public abstract class HateSelection
        {
            public abstract int GetHateTargetUid();
        }
        
        
        /// <summary>
        /// 仇恨对象选择器，选不到默认返回-1
        /// </summary>
        public class HateComp : ALogicComponent
        {
            public HateComp(ActorLogic logic, HateSelection hateSelection) : base(logic)
            {
                _hateSelection = hateSelection;
            }

            private HateSelection _hateSelection;
            
            public override void Init()
            {
                
            }

            public void UpdateHateTarget()
            {
               var hateUid= _hateSelection.GetHateTargetUid();
               Actor.SetAttr(ELogicAttr.AttrHateTargetUid, hateUid, false);
            }
        }
    }
}