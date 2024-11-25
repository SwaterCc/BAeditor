#region

using Hono.Scripts.Battle.Base;
using UnityEngine;

#endregion

namespace Hono.Scripts.Battle
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
                object variable = null;

                if (Parent.Data.NodeType == EAbilityNodeType.EAction)
                {
                    variable = ((AActionNode)Parent).FuncResult;
                }
                else
                {
                    if (!Data.Value.TryParse(AContext, out variable))
                    {
                        Debug.LogError($"函数执行失败 Name {Data.Name}");
                        return;
                    }
                }
                
                //这里的目标是持有
                AContext.Vairables.Set(Data.Name, variable);

                DoChildrenJob();
            }

            public override void Recycle()
            {
                APool<AVariableNode>.Pool.Recycle(this);
            }
        }
    }
}