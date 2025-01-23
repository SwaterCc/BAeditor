namespace Hono.Scripts.Battle.Core
{
    /// <summary>
    /// ActorActionSystem
    /// 所谓Action是将一些动作抽象成对象，使其可以组合，并行，参考来自Cocos的Action
    /// </summary>
    public class ActionSystem
    {
        private Unit _unit;
        public ActionSystem(Unit unit)
        {
            _unit = unit;
        }

        public void PlayAction(IUnitAction action)
        {
            
        }

        public void StopAction(int actionId)
        {
            
        }

        public void StopAllAction()
        {
            
        }
        
        
        public void Tick(float dt)
        {
            
        }
        
        public void Clear()
        {
            
        }
    }
}