namespace Hono.Scripts.Battle.AbilitySystem
{
    public partial class Ability
    {
        private class AMsgSendNode : ANode<MsgSendNodeData>, IAPoolObject
        {
            public override void DoJob()
            {
                
            }

            public override void Recycle()
            {
                APool<AMsgSendNode>.Pool.Recycle(this);
            }
        }
    }
}