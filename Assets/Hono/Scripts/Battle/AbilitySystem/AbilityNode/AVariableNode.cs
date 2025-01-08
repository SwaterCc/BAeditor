#region

using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle.AbilitySystem
{
    public partial class Ability
    {
        /// <summary>
        ///     变量节点，执行变量创建或者指定变量修改
        /// </summary>
        private class AVariableNode : ANode<VariableNodeData>, IAPoolObject
        {
            public override void DoJob()
            {
                

                /*if (!Data.Value.TryParse(AContext, out variable))
                    {
                        Debug.LogError($"函数执行失败 Name {Data.Key}");
                        return;
                    }*/
                
                //这里的目标是持有
                //AContext.VariableBoard.Set(Data.key, );

                DoChildrenJob();
            }

            public override void Recycle()
            {
                APool<AVariableNode>.Pool.Recycle(this);
            }
        }
    }
}