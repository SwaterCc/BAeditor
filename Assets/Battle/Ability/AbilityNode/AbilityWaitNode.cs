using Battle.Def;

namespace Battle
{
    public partial class Ability
    {
        private class AbilityWaitNode : AbilityNode,IWaitCallBack
        {
            public float waitDuration_;
            public AbilityWaitNode(AbilityExecutor executor, AbilityNodeData data) : base(executor, data) { }
            public override void DoJob()
            {
                waitDuration_ = 0;
                _executor.State.Current.Wait(this);
            }

            public bool IsWaiting()
            {
               return waitDuration_ < NodeData.WaitNodeData;
            }

            public void Add(float dt)
            {
                waitDuration_ += dt;
            }

            public void OnCallBack()
            {
                //走下个节点
            }
        }
    }
}